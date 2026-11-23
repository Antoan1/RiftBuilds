using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using RiftBuilds.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace RiftBuilds.ViewModels.Builds
{
    public class BuildQueryViewModel
    {
        [StringLength(100)]
        [Display(Name = "Search by title")]
        public string? SearchTerm { get; set; }

        [Range(1, int.MaxValue)]
        [Display(Name = "Champion")]
        public int? ChampionId { get; set; }

        [EnumDataType(typeof(Position))]
        public Position? Position { get; set; }

        [Range(1, int.MaxValue)]
        public int CurrentPage { get; set; } = 1;

        public const int PageSize = 6;

        [ValidateNever]
        public int TotalBuilds { get; set; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalBuilds / PageSize);

        [ValidateNever]
        public IEnumerable<BuildListViewModel> Builds { get; set; }
            = new List<BuildListViewModel>();

        [ValidateNever]
        public IEnumerable<SelectListItem> Champions { get; set; }
            = new List<SelectListItem>();
    }
}