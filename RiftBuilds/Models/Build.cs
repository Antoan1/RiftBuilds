using RiftBuilds.Models.Enums;
using System.ComponentModel.DataAnnotations;
namespace RiftBuilds.Models
{
    public class Build
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [EnumDataType(typeof(Position))]
        public Position Position { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public int ChampionId { get; set; }

        public Champion Champion { get; set; } = null!;

        public ICollection<BuildItem> BuildItems { get; set; } = new List<BuildItem>();
    }
}
