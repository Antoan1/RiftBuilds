using RiftBuilds.Models;
using RiftBuilds.Models.Enums;
using RiftBuilds.ViewModels.Items;

namespace RiftBuilds.Services.Interfaces
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> GetAllAsync();

        Task GetFilteredAsync(ItemQueryViewModel model);
        Task<ItemFormViewModel?> GetForEditAsync(int id);

        Task<bool> NameExistsAsync(
            string name,
            int? excludedId = null);

        Task<int> CreateAsync(ItemFormViewModel model);

        Task<bool> UpdateAsync(
            int id,
            ItemFormViewModel model);

        Task<ItemDeleteViewModel?> GetForDeleteAsync(int id);

        Task<ItemDeleteResult> DeleteAsync(int id);
    }
}