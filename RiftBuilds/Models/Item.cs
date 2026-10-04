using System.ComponentModel.DataAnnotations;
namespace RiftBuilds.Models
{
    public class Item
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Range (0, int.MaxValue)]
        public int Price { get; set; }

        [Required]
        [StringLength(200)]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
