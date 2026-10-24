using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Moq;
using Shared.Dto.Products;
using Services;
using Xunit;

namespace ECommerce.Tests
{
    public class ProductServiceTests
    {
        private readonly IMapper _mapper = TestMapper.Create();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IGenericRepository<Product, int>> _productRepository = new();

        public ProductServiceTests()
        {
            _unitOfWork
                .Setup(u => u.GetRepository<Product, int>())
                .Returns(_productRepository.Object);
        }

        private static Product CreateProduct(int id, string name, decimal price) => new()
        {
            Id = id,
            Name = name,
            Description = $"Description {name}",
            Price = price,
            PictureUrl = $"images/{name}.jpg",
            ProductBrand = new ProductBrand { Id = 1, Name = "Brand" },
            ProductType = new ProductType { Id = 1, Name = "Type" }
        };

        [Fact]
        public async Task GetAllProductsAsync_ReturnsPaginatedMappedProducts()
        {
            var products = new List<Product>
            {
                CreateProduct(1, "Product A", 10m),
                CreateProduct(2, "Product B", 20m)
            };
            _productRepository
                .Setup(r => r.GetAllAsync(It.IsAny<ISpecification<Product>>()))
                .ReturnsAsync(products);
            _productRepository
                .Setup(r => r.CountAsync(It.IsAny<ISpecification<Product>>()))
                .ReturnsAsync(2);

            var service = new ProductService(_unitOfWork.Object, _mapper);
            var result = await service.GetAllProductsAsync(new ProductQueryParameter
            {
                PageIndex = 1,
                PageSize = 10
            });

            Assert.Equal(1, result.PageIndex);
            Assert.Equal(10, result.PageSize);
            Assert.Equal(2, result.Count);
            Assert.Equal(2, result.Data.Count());
            var first = result.Data.First();
            Assert.Equal("Product A", first.Name);
            Assert.Equal(10m, first.Price);
            Assert.Equal("Brand", first.BrandName);
            Assert.Equal("Type", first.TypeName);
            Assert.Equal("http://localhost:5000images/Product A.jpg", first.PictureUrl);
        }

        [Fact]
        public async Task GetAllProductsAsync_AppliesSpecificationToRepository()
        {
            _productRepository
                .Setup(r => r.GetAllAsync(It.IsAny<ISpecification<Product>>()))
                .ReturnsAsync([]);
            _productRepository
                .Setup(r => r.CountAsync(It.IsAny<ISpecification<Product>>()))
                .ReturnsAsync(0);

            var service = new ProductService(_unitOfWork.Object, _mapper);
            await service.GetAllProductsAsync(new ProductQueryParameter { PageIndex = 2, PageSize = 5 });

            _productRepository.Verify(r => r.GetAllAsync(It.IsAny<ISpecification<Product>>()), Times.Once);
            _productRepository.Verify(r => r.CountAsync(It.IsAny<ISpecification<Product>>()), Times.Once);
        }

        [Fact]
        public async Task GetProductAsync_WhenNotFound_ThrowsProductNotFound()
        {
            _productRepository
                .Setup(r => r.GetAsync(It.IsAny<ISpecification<Product>>()))
                .ReturnsAsync((Product?)null);

            var service = new ProductService(_unitOfWork.Object, _mapper);
            var exception = await Assert.ThrowsAsync<BusinessException>(
                () => service.GetProductAsync(99));

            Assert.Equal(BusinessErrorCode.ProductNotFound, exception.ErrorCode);
        }

        [Fact]
        public async Task GetProductAsync_ReturnsMappedProduct()
        {
            _productRepository
                .Setup(r => r.GetAsync(It.IsAny<ISpecification<Product>>()))
                .ReturnsAsync(CreateProduct(7, "Product G", 15m));

            var service = new ProductService(_unitOfWork.Object, _mapper);
            var result = await service.GetProductAsync(7);

            Assert.Equal(7, result.Id);
            Assert.Equal("Product G", result.Name);
            Assert.Equal("Brand", result.BrandName);
        }

        [Fact]
        public async Task GetBrandAsync_ReturnsMappedBrands()
        {
            var brandRepository = new Mock<IGenericRepository<ProductBrand, int>>();
            brandRepository
                .Setup(r => r.GetAllAsync(It.IsAny<bool>()))
                .ReturnsAsync(new List<ProductBrand>
                {
                    new() { Id = 1, Name = "Adidas" },
                    new() { Id = 2, Name = "Nike" }
                });
            _unitOfWork
                .Setup(u => u.GetRepository<ProductBrand, int>())
                .Returns(brandRepository.Object);

            var service = new ProductService(_unitOfWork.Object, _mapper);
            var result = (await service.GetBrandAsync()).ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Adidas", result[0].Name);
        }
    }
}
