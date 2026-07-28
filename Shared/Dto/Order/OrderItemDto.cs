namespace Shared.Dto.Order
{
    public record OrderItemDto
    {
        public int Id { get; init; }
        public int ProductId { get; init; }
        public string ProductName { get; init; }
        public string PictureUrl { get; init; }
        public decimal Price { get; init; }
        public int Quantity { get; init; }
    }
}
