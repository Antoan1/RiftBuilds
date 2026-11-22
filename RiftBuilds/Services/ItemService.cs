using System.Data;
using Microsoft.EntityFrameworkCore;
using RiftBuilds.Data;
using RiftBuilds.Models;
using RiftBuilds.Models.Enums;
using RiftBuilds.Services.Interfaces;
using RiftBuilds.ViewModels.Items;

namespace RiftBuilds.Services
{
    public class ItemService : IItemService
    {
        private readonly ApplicationDbContext context;

        public ItemService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            return await context.Items
                .AsNoTracking()
                .OrderBy(item => item.Name)
                .ToListAsync();
        }

        public async Task<ItemFormViewModel?> GetForEditAsync(int id)
        {
            return await context.Items
                .AsNoTracking()
                .Where(item => item.Id == id)
                .Select(item => new ItemFormViewModel
                {
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    ImageUrl = item.ImageUrl
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> NameExistsAsync(
            string name,
            int? excludedId = null)
        {
            var normalizedName = name.Trim().ToUpperInvariant();

            return await context.Items
                .AnyAsync(item =>
                    item.Name.ToUpper() == normalizedName
                    && (!excludedId.HasValue
                        || item.Id != excludedId.Value));
        }

        public async Task<int> CreateAsync(ItemFormViewModel model)
        {
            var item = new Item();

            ApplyFormValues(item, model);

            context.Items.Add(item);
            await context.SaveChangesAsync();

            return item.Id;
        }

        public async Task<bool> UpdateAsync(
            int id,
            ItemFormViewModel model)
        {
            var item = await context.Items
                .FirstOrDefaultAsync(item => item.Id == id);

            if (item == null)
            {
                return false;
            }

            ApplyFormValues(item, model);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<ItemDeleteViewModel?> GetForDeleteAsync(int id)
        {
            return await context.Items
                .AsNoTracking()
                .Where(item => item.Id == id)
                .Select(item => new ItemDeleteViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    ImageUrl = item.ImageUrl,
                    Price = item.Price,
                    BuildsCount = item.BuildItems.Count
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ItemDeleteResult> DeleteAsync(int id)
        {
            await using var transaction = await context.Database
                .BeginTransactionAsync(IsolationLevel.Serializable);

            var item = await context.Items
                .FirstOrDefaultAsync(item => item.Id == id);

            if (item == null)
            {
                return ItemDeleteResult.NotFound;
            }

            var isUsedInBuild = await context.BuildItems
                .AnyAsync(buildItem => buildItem.ItemId == id);

            if (isUsedInBuild)
            {
                return ItemDeleteResult.InUse;
            }

            context.Items.Remove(item);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return ItemDeleteResult.Success;
        }

        private static void ApplyFormValues(
            Item item,
            ItemFormViewModel model)
        {
            item.Name = model.Name.Trim();
            item.Description = model.Description.Trim();
            item.Price = model.Price;
            item.ImageUrl = model.ImageUrl.Trim();
        }
    }
}