using System.ComponentModel.DataAnnotations;

namespace RiftBuilds.ViewModels.Champions
{
    public class ChampionFormViewModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        [Display(Name = "Image path")]
        public string ImageUrl { get; set; } = string.Empty;
    }
}