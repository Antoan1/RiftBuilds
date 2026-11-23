using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RiftBuilds.Models;

namespace RiftBuilds.ViewModels.Champions
{
    public class ChampionQueryViewModel
    {
        [StringLength(50)]
        [Display(Name = "Search by name")]
        public string? SearchTerm { get; set; }

        [Range(1, int.MaxValue)]
        public int CurrentPage { get; set; } = 1;

        public const int PageSize = 9;

        [ValidateNever]
        public int TotalChampions { get; set; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalChampions / PageSize);

        [ValidateNever]
        public IEnumerable<Champion> Champions { get; set; }
            = new List<Champion>();
    }
}