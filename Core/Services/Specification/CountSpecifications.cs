using Domain.Models;
using Shared.Dto.Products;
using System.Linq.Expressions;

namespace Services.Specification
{
    internal class ProductCountSpecifications(ProductQueryParameter parameter) : BaseSpecification<Product>(CreateCriteria(parameter))
    {
        private static Expression<Func<Product, bool>>
            CreateCriteria(ProductQueryParameter parameter)
        {
            return product =>
                (!parameter.BrandId.HasValue || product.BrandId == parameter.BrandId.Value) &&
                (!parameter.TypeId.HasValue || product.TypeId == parameter.TypeId.Value) &&
                (string.IsNullOrWhiteSpace(parameter.Search) || product.Name.ToLower().Contains(parameter.Search.ToLower()));
        }
    }
}
