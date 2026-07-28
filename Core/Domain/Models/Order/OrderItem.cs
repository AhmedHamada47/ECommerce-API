namespace Domain.Models.Order
{
    public class OrderItem : BaseEntity<int>
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
