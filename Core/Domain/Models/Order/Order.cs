namespace Domain.Models.Order
{
    public class Order : BaseEntity<int>
    {
        public string BuyerEmail { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public ShippingAddress ShippingAddress { get; set; } = default!;
        public int DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; } = default!;
        public ICollection<OrderItem> OrderItems { get; set; } = [];
        public decimal Subtotal { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string PaymentIntentId { get; set; } = string.Empty;

        public decimal GetTotal() => Subtotal + DeliveryMethod.Price;
    }
}
