using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Order
{
    public record ShippingAddressDto
    {
        [Required(ErrorMessage = "FirstName is required")]
        [MaxLength(100)]
        public string FirstName { get; init; }

        [Required(ErrorMessage = "LastName is required")]
        [MaxLength(100)]
        public string LastName { get; init; }

        [Required(ErrorMessage = "Street is required")]
        [MaxLength(200)]
        public string Street { get; init; }

        [Required(ErrorMessage = "City is required")]
        [MaxLength(100)]
        public string City { get; init; }

        [Required(ErrorMessage = "State is required")]
        [MaxLength(100)]
        public string State { get; init; }

        [Required(ErrorMessage = "ZipCode is required")]
        [MaxLength(20)]
        public string ZipCode { get; init; }
    }
}
