
using RiftBuilds.Models;

namespace RiftBuilds.Services.Interfaces
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> GetAllAsync();
    }
}
