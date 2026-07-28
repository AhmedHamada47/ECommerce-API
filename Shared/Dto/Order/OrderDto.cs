namespace Shared.Dto.Order
{
    public record OrderDto
    {
        public int Id { get; init; }
        public string BuyerEmail { get; init; }
        public DateTimeOffset OrderDate { get; init; }
        public ShippingAddressDto ShippingAddress { get; init; }
        public string DeliveryMethod { get; init; }
        public decimal DeliveryPrice { get; init; }
        public ICollection<OrderItemDto> OrderItems { get; init; }
        public decimal Subtotal { get; init; }
        public decimal Total { get; init; }
        public string Status { get; init; }
    }
}
