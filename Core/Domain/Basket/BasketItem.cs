namespace Domain.Basket
{
    public class BasketItem
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public string ImageUrl => $"{PictureUrl}";

        public decimal TotalPrice => Price * Quantity;
    }
}
