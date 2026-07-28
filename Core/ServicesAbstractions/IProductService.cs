using Shared.Dto;
using Shared.Dto.Products;

namespace ServicesAbstractions
{
    public interface IProductService
    {
        Task<PaginatedResponse<ProductResponse>> GetAllProductsAsync(ProductQueryParameter parameter);
        Task<ProductResponse> GetProductAsync(int id);
        Task<IEnumerable<BrandResponse>> GetBrandAsync();
        Task<IEnumerable<TypeResponse>> GetTypeAsync();
    }
}
