using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RiftBuilds.Data;
using RiftBuilds.Services.Interfaces;
using RiftBuilds.ViewModels.Builds;
using RiftBuilds.Models;

namespace RiftBuilds.Services
{
    public class BuildService : IBuildService
    {
        private readonly ApplicationDbContext context;

        public BuildService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task PopulateOptionsAsync(BuildFormViewModel model)
        {
            model.Champions = await context.Champions
                .AsNoTracking()
                .OrderBy(champion => champion.Name)
                .Select(champion => new SelectListItem
                {
                    Value = champion.Id.ToString(),
                    Text = champion.Name
                })
                .ToListAsync();

            model.Items = await context.Items
                .AsNoTracking()
                .OrderBy(item => item.Name)
                .Select(item => new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Name
                })
                .ToListAsync();
        }

        public async Task<bool> ChampionExistsAsync(int championId)
        {
            return await context.Champions
                .AnyAsync(champion => champion.Id == championId);
        }

        public async Task<bool> ItemsExistAsync(int[] itemIds)
        {
            var uniqueIds = itemIds.Distinct().ToArray();

            var existingCount = await context.Items
                .CountAsync(item => uniqueIds.Contains(item.Id));

            return existingCount == uniqueIds.Length;
        }

        public async Task<int> CreateAsync(BuildFormViewModel model)
        {
            var build = new Build
            {
                Title = model.Title.Trim(),
                Description = model.Description.Trim(),
                ChampionId = model.ChampionId,
                Position = model.Position,
                CreatedOn = DateTime.UtcNow
            };

            for (int i = 0; i < model.SelectedItemIds.Length; i++)
            {
                build.BuildItems.Add(new BuildItem
                {
                    ItemId = model.SelectedItemIds[i],
                    Slot = i + 1
                });
            }

            context.Builds.Add(build);
            await context.SaveChangesAsync();

            return build.Id;
        }
    }
}