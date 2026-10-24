using AutoMapper;
using Domain.Basket;
using Domain.Contracts;
using Domain.Exceptions;
using Moq;
using Services;
using Xunit;

namespace ECommerce.Tests
{
    public class BasketServiceTests
    {
        private readonly IMapper _mapper = TestMapper.Create();
        private readonly Mock<IBasketRepository> _basketRepository = new();

        [Fact]
        public async Task UpdateAsync_PersistsBasketAndReturnsMappedDto()
        {
            var basketDto = new BasketDtoBuilder()
                .WithItem(1, "Product A", 10m, 2)
                .Build();
            var stored = _mapper.Map<CustomerBasket>(basketDto);

            _basketRepository
                .Setup(r => r.UpdateAsync(It.IsAny<CustomerBasket>(), It.IsAny<TimeSpan?>()))
                .ReturnsAsync(stored);

            var service = new BasketService(_basketRepository.Object, _mapper);
            var result = await service.UpdateAsync(basketDto);

            _basketRepository.Verify(
                r => r.UpdateAsync(It.Is<CustomerBasket>(b => b.Id == basketDto.Id), It.IsAny<TimeSpan?>()),
                Times.Once);
            Assert.Equal(basketDto.Id, result.Id);
            Assert.Single(result.BasketItems);
            Assert.Equal("Product A", result.BasketItems.First().ProductName);
            Assert.Equal(2, result.BasketItems.First().Quantity);
        }

        [Fact]
        public async Task UpdateAsync_WhenRepositoryReturnsNull_ThrowsBasketUpdateFailed()
        {
            _basketRepository
                .Setup(r => r.UpdateAsync(It.IsAny<CustomerBasket>(), It.IsAny<TimeSpan?>()))
                .ReturnsAsync((CustomerBasket?)null);

            var service = new BasketService(_basketRepository.Object, _mapper);
            var exception = await Assert.ThrowsAsync<BusinessException>(
                () => service.UpdateAsync(new BasketDtoBuilder().Build()));

            Assert.Equal(BusinessErrorCode.BasketUpdateFailed, exception.ErrorCode);
        }

        [Fact]
        public async Task GetAsync_WhenBasketDoesNotExist_ThrowsBasketNotFound()
        {
            _basketRepository
                .Setup(r => r.GetAsync(It.IsAny<string>()))
                .ReturnsAsync((CustomerBasket?)null);

            var service = new BasketService(_basketRepository.Object, _mapper);
            var exception = await Assert.ThrowsAsync<BusinessException>(
                () => service.GetAsync("missing-basket"));

            Assert.Equal(BusinessErrorCode.BasketNotFound, exception.ErrorCode);
        }

        [Fact]
        public async Task GetAsync_ReturnsMappedBasket()
        {
            var basket = new CustomerBasket
            {
                Id = "basket-1",
                Items =
                [
                    new BasketItem { Id = 1, ProductName = "Product A", PictureUrl = "a.jpg", Price = 10m, Quantity = 1 }
                ]
            };
            _basketRepository
                .Setup(r => r.GetAsync("basket-1"))
                .ReturnsAsync(basket);

            var service = new BasketService(_basketRepository.Object, _mapper);
            var result = await service.GetAsync("basket-1");

            Assert.Equal("basket-1", result.Id);
            Assert.Single(result.BasketItems);
        }

        [Fact]
        public async Task DeleteAsync_DelegatesToRepository()
        {
            _basketRepository
                .Setup(r => r.DeleteAsync("basket-1"))
                .ReturnsAsync(true);

            var service = new BasketService(_basketRepository.Object, _mapper);
            var result = await service.DeleteAsync("basket-1");

            Assert.True(result);
            _basketRepository.Verify(r => r.DeleteAsync("basket-1"), Times.Once);
        }
    }
}
