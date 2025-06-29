using Domain.Contracts;
using Shared.Dto.Products;
using System.Linq.Expressions;

namespace Services.Specefication
{
    internal class ProductCountSpecefications(ProductQueryParameter parameter) : BaseSpecification<Product>(CreateCriteria (parameter))
    {
        private static Expression<Func<Product, bool>> 
            CreateCriteria(ProductQueryParameter parameter)
        {
            return product =>
                (!parameter.BrandId.HasValue || product.BrandId == parameter.BrandId.Value) &&
                (!parameter.Typeid.HasValue || product.TypeId == parameter.Typeid.Value) &&
                (string.IsNullOrWhiteSpace(parameter.Search) || product.Name.ToLower().Contains(parameter.Search.ToLower()));
        }
    }
}
