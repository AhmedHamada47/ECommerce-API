using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Products
{
    public record ProductUpsertDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100)]
        public string Name { get; init; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500)]
        public string Description { get; init; }

        [MaxLength(500)]
        public string PictureUrl { get; init; }

        [Range(0, 9999999, ErrorMessage = "Price must be between 0 and 9999999")]
        public decimal Price { get; init; }

        [Range(1, int.MaxValue, ErrorMessage = "BrandId is required")]
        public int BrandId { get; init; }

        [Range(1, int.MaxValue, ErrorMessage = "TypeId is required")]
        public int TypeId { get; init; }
    }
}
