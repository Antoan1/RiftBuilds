using Microsoft.EntityFrameworkCore;
using RiftBuilds.Data;
using RiftBuilds.Models;
using RiftBuilds.Services.Interfaces;
using RiftBuilds.ViewModels.Champions;
using System.Data;
using RiftBuilds.Models.Enums;

namespace RiftBuilds.Services
{
    public class ChampionService : IChampionService
    {
        private readonly ApplicationDbContext context;

        public ChampionService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Champion>> GetAllAsync()
        {
            return await context.Champions
                .AsNoTracking()
                .OrderBy(champion => champion.Name)
                .ToListAsync();
        }

        public async Task<ChampionFormViewModel?> GetForEditAsync(int id)
        {
            return await context.Champions
                .AsNoTracking()
                .Where(champion => champion.Id == id)
                .Select(champion => new ChampionFormViewModel
                {
                    Name = champion.Name,
                    Description = champion.Description,
                    ImageUrl = champion.ImageUrl
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> NameExistsAsync(
            string name,
            int? excludedId = null)
        {
            var normalizedName = name.Trim().ToUpperInvariant();

            return await context.Champions
                .AnyAsync(champion =>
                    champion.Name.ToUpper() == normalizedName
                    && (!excludedId.HasValue
                        || champion.Id != excludedId.Value));
        }

        public async Task<int> CreateAsync(
            ChampionFormViewModel model)
        {
            var champion = new Champion();

            ApplyFormValues(champion, model);

            context.Champions.Add(champion);
            await context.SaveChangesAsync();

            return champion.Id;
        }

        public async Task<bool> UpdateAsync(
            int id,
            ChampionFormViewModel model)
        {
            var champion = await context.Champions
                .FirstOrDefaultAsync(champion => champion.Id == id);

            if (champion == null)
            {
                return false;
            }

            ApplyFormValues(champion, model);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<ChampionDeleteViewModel?> GetForDeleteAsync(int id)
        {
            return await context.Champions
                .AsNoTracking()
                .Where(champion => champion.Id == id)
                .Select(champion => new ChampionDeleteViewModel
                {
                    Id = champion.Id,
                    Name = champion.Name,
                    ImageUrl = champion.ImageUrl,
                    BuildsCount = champion.Builds.Count
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ChampionDeleteResult> DeleteAsync(int id)
        {
            await using var transaction = await context.Database
                .BeginTransactionAsync(IsolationLevel.Serializable);

            var champion = await context.Champions
                .FirstOrDefaultAsync(champion => champion.Id == id);

            if (champion == null)
            {
                return ChampionDeleteResult.NotFound;
            }

            var isUsedInBuild = await context.Builds
                .AnyAsync(build => build.ChampionId == id);

            if (isUsedInBuild)
            {
                return ChampionDeleteResult.InUse;
            }

            context.Champions.Remove(champion);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return ChampionDeleteResult.Success;
        }

        private static void ApplyFormValues(
            Champion champion,
            ChampionFormViewModel model)
        {
            champion.Name = model.Name.Trim();
            champion.Description = model.Description.Trim();
            champion.ImageUrl = model.ImageUrl.Trim();
        }
    }
}