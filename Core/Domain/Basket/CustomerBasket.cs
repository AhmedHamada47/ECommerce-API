namespace Domain.Basket
{
    public class CustomerBasket
    {
        public string Id { get; set; }
        public ICollection<BasketItem> Items { get; set; } = [];
    }
}
