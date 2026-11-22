namespace RiftBuilds.ViewModels.Champions
{
    public class ChampionDeleteViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public int BuildsCount { get; set; }

        public bool CanDelete => BuildsCount == 0;
    }
}