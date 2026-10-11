using RiftBuilds.Models.Enums;

namespace RiftBuilds.ViewModels.Builds
{
    public class BuildListViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string ChampionName { get; set; } = string.Empty;

        public string ChampionImageUrl { get; set; } = string.Empty;

        public Position Position { get; set; }
    }
}