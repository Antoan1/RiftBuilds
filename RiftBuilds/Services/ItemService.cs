using Microsoft.EntityFrameworkCore;
using RiftBuilds.Data;
using RiftBuilds.Models;
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