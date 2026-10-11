using RiftBuilds.Models.Enums;

namespace RiftBuilds.ViewModels.Builds
{
    public class BuildDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ChampionName { get; set; } = string.Empty;

        public string ChampionImageUrl { get; set; } = string.Empty;

        public Position Position { get; set; }

        public DateTime CreatedOn { get; set; }

        public List<BuildItemViewModel> Items { get; set; }
            = new List<BuildItemViewModel>();

        public int TotalPrice => Items.Sum(item => item.Price);
    }
}