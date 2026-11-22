using System.ComponentModel.DataAnnotations;

namespace RiftBuilds.ViewModels.Items
{
    public class ItemFormViewModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Range(0, int.MaxValue,
            ErrorMessage = "Price cannot be negative.")]
        public int Price { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Image path")]
        public string ImageUrl { get; set; } = string.Empty;
    }
}