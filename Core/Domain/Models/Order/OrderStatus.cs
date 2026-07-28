namespace Domain.Models.Order
{
    public enum OrderStatus
    {
        Pending,
        PaymentReceived,
        PaymentFailed,
        Shipped,
        Delivered,
        Cancelled
    }
}
