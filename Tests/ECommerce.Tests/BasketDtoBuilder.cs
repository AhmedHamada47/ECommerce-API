using Shared.Dto.Basket;

namespace ECommerce.Tests
{
    internal class BasketDtoBuilder
    {
        private readonly BasketDto _basket = new()
        {
            Id = "basket-1",
            BasketItems = []
        };

        public BasketDtoBuilder WithItem(int id, string productName, decimal price, int quantity)
        {
            _basket.BasketItems.Add(new BasketItemDto
            {
                Id = id,
                ProductName = productName,
                PictureUrl = $"{productName}.jpg",
                Price = price,
                Quantity = quantity
            });
            return this;
        }

        public BasketDto Build() => _basket;
    }
}
