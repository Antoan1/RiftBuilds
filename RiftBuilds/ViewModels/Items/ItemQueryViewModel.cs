using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RiftBuilds.Models;

namespace RiftBuilds.ViewModels.Items
{
    public class ItemQueryViewModel : IValidatableObject
    {
        [StringLength(50)]
        [Display(Name = "Search by name")]
        public string? SearchTerm { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Minimum price")]
        public int? MinPrice { get; set; }

        [Range(0, int.MaxValue)]
        [Display(Name = "Maximum price")]
        public int? MaxPrice { get; set; }

        [Range(1, int.MaxValue)]
        public int CurrentPage { get; set; } = 1;

        public const int PageSize = 9;

        [ValidateNever]
        public int TotalItems { get; set; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalItems / PageSize);

        [ValidateNever]
        public IEnumerable<Item> Items { get; set; }
            = new List<Item>();

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (MinPrice.HasValue
                && MaxPrice.HasValue
                && MinPrice.Value > MaxPrice.Value)
            {
                yield return new ValidationResult(
                    "Maximum price must be greater than or equal to minimum price.",
                    new[] { nameof(MaxPrice) });
            }
        }
    }
}