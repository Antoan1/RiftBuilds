using RiftBuilds.Models;
using RiftBuilds.Models.Enums;
using RiftBuilds.ViewModels.Champions;

namespace RiftBuilds.Services.Interfaces
{
    public interface IChampionService
    {
        Task<IEnumerable<Champion>> GetAllAsync();

        Task<ChampionFormViewModel?> GetForEditAsync(int id);

        Task<bool> NameExistsAsync(
            string name,
            int? excludedId = null);

        Task<int> CreateAsync(ChampionFormViewModel model);

        Task<bool> UpdateAsync(
            int id,
            ChampionFormViewModel model);

        Task<ChampionDeleteViewModel?> GetForDeleteAsync(int id);

        Task<ChampionDeleteResult> DeleteAsync(int id);
    }
}