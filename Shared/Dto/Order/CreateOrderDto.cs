using Shared.Dto.Basket;

namespace Shared.Dto.Order
{
    public record CreateOrderDto
    {
        public string BasketId { get; init; }
        public int DeliveryMethodId { get; init; }
        public ShippingAddressDto ShippingAddress { get; init; }
    }
}
