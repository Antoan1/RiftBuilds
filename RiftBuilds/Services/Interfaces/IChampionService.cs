using RiftBuilds.Models;

namespace RiftBuilds.Services.Interfaces
{
    public interface IChampionService
    {
        Task<IEnumerable<Champion>> GetAllAsync();
    }
}