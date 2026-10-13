using System.ComponentModel.DataAnnotations;
using RiftBuilds.Models.Enums;

namespace RiftBuilds.Models
{
    public class Build
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [EnumDataType(typeof(Position))]
        public Position Position { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public int ChampionId { get; set; }

        public Champion Champion { get; set; } = null!;

        public ICollection<BuildItem> BuildItems { get; set; }
            = new List<BuildItem>();
    }
}