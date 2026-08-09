using System.ComponentModel.DataAnnotations;

namespace Shared.Dto.Order
{
    public record CreateOrderDto
    {
        [Required(ErrorMessage = "BasketId is required")]
        public string BasketId { get; init; }

        [Range(1, int.MaxValue, ErrorMessage = "DeliveryMethodId is required")]
        public int DeliveryMethodId { get; init; }

        [Required(ErrorMessage = "ShippingAddress is required")]
        public ShippingAddressDto ShippingAddress { get; init; }
    }
}
