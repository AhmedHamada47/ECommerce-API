using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models;
using Services.Specification;
using Shared.Dto;
using Shared.Dto.Products;
using ServicesAbstractions;

namespace Services
{
    public class ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        : IProductService
    {
        public async Task<PaginatedResponse<ProductResponse>> GetAllProductsAsync(ProductQueryParameter parameter)
        {
            var specification = new ProductWithBrandAndTypeSpecifications(parameter);
            var result = await unitOfWork.GetRepository<Product, int>().GetAllAsync(specification);

            var data = mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponse>>(result);

            var totalCount = await unitOfWork.GetRepository<Product, int>()
                .CountAsync(new ProductCountSpecifications(parameter));

            return new(parameter.PageIndex, parameter.PageSize, totalCount, data);
        }

        public async Task<ProductResponse> GetProductAsync(int id)
        {
            var specification = new ProductWithBrandAndTypeSpecifications(id);
            var product = await unitOfWork.GetRepository<Product, int>().GetAsync(specification)
                ?? throw new BusinessException(BusinessErrorCode.ProductNotFound, id);
            return mapper.Map<Product, ProductResponse>(product);
        }

        public async Task<IEnumerable<BrandResponse>> GetBrandAsync()
        {
            var repo = unitOfWork.GetRepository<ProductBrand, int>();
            var brands = await repo.GetAllAsync();
            var result = mapper.Map<IEnumerable<ProductBrand>, IEnumerable<BrandResponse>>(brands);
            return result;
        }

        public async Task<IEnumerable<TypeResponse>> GetTypeAsync()
        {
            var repo = unitOfWork.GetRepository<ProductType, int>();
            var types = await repo.GetAllAsync();
            var result = mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeResponse>>(types);
            return result;
        }
    }
}
