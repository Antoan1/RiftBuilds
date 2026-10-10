using Microsoft.EntityFrameworkCore;
using RiftBuilds.Data;
using RiftBuilds.Models;
using RiftBuilds.Services.Interfaces;

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
    }
}