using Shared.Dto;
using Shared.Dto.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstractions
{
    public interface IProductService
    {
        Task<PagenatedRespones<ProductsRespone>> GetAllProductsAsync(ProductQueryParameter parameter);

        Task<ProductsRespone> GetProductAsync(int id);

        Task<IEnumerable<BrandResponse>> GetBrandAsync();
        Task<IEnumerable<TypeResponse>> GetTypeAsync();
    }
}
