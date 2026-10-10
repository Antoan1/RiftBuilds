using System.ComponentModel.DataAnnotations;
using RiftBuilds.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace RiftBuilds.ViewModels.Builds
{
    public class BuildFormViewModel
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Please select a champion.")]
        public int ChampionId { get; set; }

        [EnumDataType(typeof(Position),
          ErrorMessage = "Please select a valid position.")]
        public Position Position { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Please select at least one item.")]
        [MaxLength(6, ErrorMessage = "You can select up to six items.")]
        public int[] SelectedItemIds { get; set; } = Array.Empty<int>();

        [ValidateNever]
        public IEnumerable<SelectListItem> Champions { get; set; }
            = new List<SelectListItem>();

        [ValidateNever]
        public IEnumerable<SelectListItem> Items { get; set; }
            = new List<SelectListItem>();
    }
}
