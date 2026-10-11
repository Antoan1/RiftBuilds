using RiftBuilds.ViewModels.Builds;

namespace RiftBuilds.Services.Interfaces
{
    public interface IBuildService
    {
        Task PopulateOptionsAsync (BuildFormViewModel model);

        Task<bool> ChampionExistsAsync(int championId);

        Task<bool> ItemsExistAsync(int[] itemIds);

        Task<int> CreateAsync(BuildFormViewModel model);

        Task<IEnumerable<BuildListViewModel>> GetAllAsync();
    }
}
