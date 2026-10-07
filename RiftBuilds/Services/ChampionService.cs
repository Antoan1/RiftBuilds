using Microsoft.EntityFrameworkCore;
using RiftBuilds.Data;
using RiftBuilds.Models;
using RiftBuilds.Services.Interfaces;


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
    }
}
