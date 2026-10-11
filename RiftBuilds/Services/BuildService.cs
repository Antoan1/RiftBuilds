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

        public async Task<IEnumerable<BuildListViewModel>> GetAllAsync()
        {
            return await context.Builds
                .AsNoTracking()
                .OrderByDescending(build => build.CreatedOn)
                .ThenByDescending(build => build.Id)
                .Select(build => new BuildListViewModel
                {
                    Id = build.Id,
                    Title = build.Title,
                    ChampionName = build.Champion.Name,
                    ChampionImageUrl = build.Champion.ImageUrl,
                    Position = build.Position
                })
                .ToListAsync();
        }

        public async Task<BuildDetailsViewModel?> GetDetailsAsync(int id)
        {
            return await context.Builds
                .AsNoTracking()
                .Where(build => build.Id == id)
                .Select(build => new BuildDetailsViewModel
                {
                    Id = build.Id,
                    Title = build.Title,
                    Description = build.Description,
                    ChampionName = build.Champion.Name,
                    ChampionImageUrl = build.Champion.ImageUrl,
                    Position = build.Position,
                    CreatedOn = build.CreatedOn,
                    Items = build.BuildItems
                        .OrderBy(buildItem => buildItem.Slot)
                        .Select(buildItem => new BuildItemViewModel
                        {
                            Name = buildItem.Item.Name,
                            ImageUrl = buildItem.Item.ImageUrl,
                            Price = buildItem.Item.Price,
                            Slot = buildItem.Slot
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }
        public async Task<BuildFormViewModel?> GetForEditAsync(int id)
        {
            var model = await context.Builds
                .AsNoTracking()
                .Where(build => build.Id == id)
                .Select(build => new BuildFormViewModel
                {
                    Title = build.Title,
                    Description = build.Description,
                    ChampionId = build.ChampionId,
                    Position = build.Position,
                    SelectedItemIds = build.BuildItems
                        .OrderBy(buildItem => buildItem.Slot)
                        .Select(buildItem => buildItem.ItemId)
                        .ToArray()
                })
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return null;
            }

            await PopulateOptionsAsync(model);

            return model;
        }
        public async Task<bool> UpdateAsync(
    int id,
    BuildFormViewModel model)
        {
            var build = await context.Builds
                .Include(build => build.BuildItems)
                .FirstOrDefaultAsync(build => build.Id == id);

            if (build == null)
            {
                return false;
            }

            build.Title = model.Title.Trim();
            build.Description = model.Description.Trim();
            build.ChampionId = model.ChampionId;
            build.Position = model.Position;

            var selectedIds = model.SelectedItemIds;

            var itemsToRemove = build.BuildItems
                .Where(buildItem => !selectedIds.Contains(buildItem.ItemId))
                .ToList();

            context.BuildItems.RemoveRange(itemsToRemove);

            for (int i = 0; i < selectedIds.Length; i++)
            {
                var existingItem = build.BuildItems
                    .FirstOrDefault(buildItem =>
                        buildItem.ItemId == selectedIds[i]);

                if (existingItem != null)
                {
                    existingItem.Slot = i + 1;
                }
                else
                {
                    build.BuildItems.Add(new BuildItem
                    {
                        ItemId = selectedIds[i],
                        Slot = i + 1
                    });
                }
            }

            await context.SaveChangesAsync();

            return true;
        }
    }
}