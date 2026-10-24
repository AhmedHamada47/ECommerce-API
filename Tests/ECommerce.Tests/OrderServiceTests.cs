using AutoMapper;
using Domain.Basket;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Domain.Models.Order;
using Moq;
using Shared.Dto.Order;
using Services;
using Xunit;

namespace ECommerce.Tests
{
    public class OrderServiceTests
    {
        private readonly IMapper _mapper = TestMapper.Create();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IBasketRepository> _basketRepository = new();
        private readonly Mock<IGenericRepository<DeliveryMethod, int>> _deliveryMethodRepository = new();
        private readonly Mock<IGenericRepository<Product, int>> _productRepository = new();
        private readonly Mock<IGenericRepository<Order, int>> _orderRepository = new();

        public OrderServiceTests()
        {
            _unitOfWork.Setup(u => u.GetRepository<DeliveryMethod, int>()).Returns(_deliveryMethodRepository.Object);
            _unitOfWork.Setup(u => u.GetRepository<Product, int>()).Returns(_productRepository.Object);
            _unitOfWork.Setup(u => u.GetRepository<Order, int>()).Returns(_orderRepository.Object);
        }

        private static CustomerBasket CreateBasket() => new()
        {
            Id = "basket-1",
            Items =
            [
                new BasketItem { Id = 1, ProductName = "Product A", PictureUrl = "a.jpg", Price = 10m, Quantity = 2 },
                new BasketItem { Id = 2, ProductName = "Product B", PictureUrl = "b.jpg", Price = 5m, Quantity = 1 }
            ]
        };

        private static Product CreateProduct(int id, string name, decimal price) => new()
        {
            Id = id,
            Name = name,
            Description = $"Description {name}",
            Price = price,
            PictureUrl = $"{name}.jpg",
            ProductBrand = new ProductBrand { Id = 1, Name = "Brand" },
            ProductType = new ProductType { Id = 1, Name = "Type" }
        };

        [Fact]
        public async Task CreateOrderAsync_ComputesSubtotalAndDeletesBasket()
        {
            _basketRepository.Setup(r => r.GetAsync("basket-1")).ReturnsAsync(CreateBasket());
            _deliveryMethodRepository
                .Setup(r => r.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(new DeliveryMethod { Id = 1, ShortName = "DHL", Price = 5m });
            _productRepository
                .Setup(r => r.GetAllAsync(It.IsAny<ISpecification<Product>>()))
                .ReturnsAsync(
                [
                    CreateProduct(1, "Product A", 10m),
                    CreateProduct(2, "Product B", 5m)
                ]);
            _unitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var service = new OrderService(_unitOfWork.Object, _basketRepository.Object, _mapper);
            var result = await service.CreateOrderAsync(
                "buyer@test.com",
                deliveryMethodId: 1,
                basketId: "basket-1",
                new ShippingAddressDto
                {
                    FirstName = "John",
                    LastName = "Doe",
                    City = "Cairo",
                    Street = "Main St"
                });

            Assert.Equal("buyer@test.com", result.BuyerEmail);
            Assert.Equal(25m, result.Subtotal);
            Assert.Equal(5m, result.DeliveryPrice);
            Assert.Equal(30m, result.Total);
            Assert.Equal(2, result.OrderItems.Count);
            Assert.Equal("DHL", result.DeliveryMethod);
            Assert.Equal("Pending", result.Status);

            _orderRepository.Verify(r => r.Add(It.Is<Order>(o =>
                o.Subtotal == 25m &&
                o.Status == OrderStatus.Pending &&
                o.OrderItems.Count == 2)), Times.Once);
            _unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
            _basketRepository.Verify(r => r.DeleteAsync("basket-1"), Times.Once);
        }

        [Fact]
        public async Task CreateOrderAsync_IgnoresBasketItemsWithoutMatchingProduct()
        {
            _basketRepository.Setup(r => r.GetAsync("basket-1")).ReturnsAsync(CreateBasket());
            _deliveryMethodRepository
                .Setup(r => r.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(new DeliveryMethod { Id = 1, ShortName = "DHL", Price = 0m });
            _productRepository
                .Setup(r => r.GetAllAsync(It.IsAny<ISpecification<Product>>()))
                .ReturnsAsync([CreateProduct(1, "Product A", 10m)]);
            _unitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var service = new OrderService(_unitOfWork.Object, _basketRepository.Object, _mapper);
            var result = await service.CreateOrderAsync(
                "buyer@test.com",
                deliveryMethodId: 1,
                basketId: "basket-1",
                new ShippingAddressDto
                {
                    FirstName = "John",
                    LastName = "Doe",
                    City = "Cairo",
                    Street = "Main St"
                });

            Assert.Single(result.OrderItems);
            Assert.Equal(20m, result.Subtotal);
        }

        [Fact]
        public async Task CreateOrderAsync_WhenBasketNotFound_ThrowsBasketNotFound()
        {
            _basketRepository
                .Setup(r => r.GetAsync("missing-basket"))
                .ReturnsAsync((CustomerBasket?)null);

            var service = new OrderService(_unitOfWork.Object, _basketRepository.Object, _mapper);
            var exception = await Assert.ThrowsAsync<BusinessException>(
                () => service.CreateOrderAsync(
                    "buyer@test.com",
                    deliveryMethodId: 1,
                    basketId: "missing-basket",
                    new ShippingAddressDto
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        City = "Cairo",
                        Street = "Main St"
                    }));

            Assert.Equal(BusinessErrorCode.BasketNotFound, exception.ErrorCode);
        }

        [Fact]
        public async Task CreateOrderAsync_WhenDeliveryMethodNotFound_ThrowsDeliveryMethodNotFound()
        {
            _basketRepository.Setup(r => r.GetAsync("basket-1")).ReturnsAsync(CreateBasket());
            _deliveryMethodRepository
                .Setup(r => r.GetAsync(It.IsAny<int>()))
                .ReturnsAsync((DeliveryMethod?)null);

            var service = new OrderService(_unitOfWork.Object, _basketRepository.Object, _mapper);
            var exception = await Assert.ThrowsAsync<BusinessException>(
                () => service.CreateOrderAsync(
                    "buyer@test.com",
                    deliveryMethodId: 999,
                    basketId: "basket-1",
                    new ShippingAddressDto
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        City = "Cairo",
                        Street = "Main St"
                    }));

            Assert.Equal(BusinessErrorCode.DeliveryMethodNotFound, exception.ErrorCode);
        }

        [Fact]
        public async Task GetOrdersForUserAsync_ReturnsOrdersForUser()
        {
            var order = new Order
            {
                Id = 1,
                BuyerEmail = "buyer@test.com",
                ShippingAddress = new ShippingAddress { FirstName = "John", LastName = "Doe", City = "Cairo", Street = "Main St" },
                DeliveryMethod = new DeliveryMethod { ShortName = "DHL", Price = 5m },
                OrderItems = [],
                Subtotal = 25m,
                Status = OrderStatus.Pending
            };
            _orderRepository
                .Setup(r => r.GetAllAsync(It.IsAny<ISpecification<Order>>()))
                .ReturnsAsync([order]);

            var service = new OrderService(_unitOfWork.Object, _basketRepository.Object, _mapper);
            var result = (await service.GetOrdersForUserAsync("buyer@test.com")).ToList();

            Assert.Single(result);
            Assert.Equal("buyer@test.com", result[0].BuyerEmail);
            Assert.Equal(30m, result[0].Total);
            _orderRepository.Verify(
                r => r.GetAllAsync(It.IsAny<ISpecification<Order>>()),
                Times.Once);
        }
    }
}
