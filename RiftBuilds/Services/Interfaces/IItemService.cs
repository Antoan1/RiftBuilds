using RiftBuilds.Models;
using RiftBuilds.ViewModels.Items;

namespace RiftBuilds.Services.Interfaces
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> GetAllAsync();

        Task<ItemFormViewModel?> GetForEditAsync(int id);

        Task<bool> NameExistsAsync(
            string name,
            int? excludedId = null);

        Task<int> CreateAsync(ItemFormViewModel model);

        Task<bool> UpdateAsync(
            int id,
            ItemFormViewModel model);
    }
}