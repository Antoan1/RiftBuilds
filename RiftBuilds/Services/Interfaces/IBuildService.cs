using RiftBuilds.ViewModels.Builds;

namespace RiftBuilds.Services.Interfaces
{
    public interface IBuildService
    {
        Task PopulateOptionsAsync(BuildFormViewModel model);

        Task<bool> ChampionExistsAsync(int championId);

        Task<bool> ItemsExistAsync(int[] itemIds);

        Task<int> CreateAsync(
            BuildFormViewModel model,
            string userId);

        Task<IEnumerable<BuildListViewModel>> GetAllAsync();

        Task<IEnumerable<BuildListViewModel>> GetMineAsync(string userId);

        Task<BuildDetailsViewModel?> GetDetailsAsync(int id);

        Task<bool> CanManageAsync(
            int id,
            string userId,
            bool isAdministrator);

        Task<BuildFormViewModel?> GetForEditAsync(
            int id,
            string userId,
            bool isAdministrator);

        Task<bool> UpdateAsync(
            int id,
            BuildFormViewModel model,
            string userId,
            bool isAdministrator);

        Task<bool> DeleteAsync(
            int id,
            string userId,
            bool isAdministrator);
    }
}