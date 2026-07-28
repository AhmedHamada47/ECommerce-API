using Domain.Models;
using Shared.Dto.Products;
using System.Linq.Expressions;

namespace Services.Specification
{
    internal class ProductWithBrandAndTypeSpecifications
        : BaseSpecification<Product>
    {
        public ProductWithBrandAndTypeSpecifications(int id)
            : base(criteria: product => product.Id == id)
        {
            AddInclude(product => product.ProductBrand);
            AddInclude(product => product.ProductType);
        }

        public ProductWithBrandAndTypeSpecifications(ProductQueryParameter parameter) : base(CreateCriteria(parameter))
        {
            AddInclude(product => product.ProductBrand);
            AddInclude(product => product.ProductType);
            ApplySorting(parameter.SortBy);
            ApplyPagination(parameter.PageSize, parameter.PageIndex);
        }

        private static Expression<Func<Product, bool>> CreateCriteria(ProductQueryParameter parameter)
        {
            return product =>
                 (!parameter.BrandId.HasValue || product.BrandId == parameter.BrandId.Value) &&
                 (!parameter.TypeId.HasValue || product.TypeId == parameter.TypeId.Value) &&
                 (string.IsNullOrWhiteSpace(parameter.Search) || product.Name.ToLower().Contains(parameter.Search.ToLower()));
        }

        private void ApplySorting(ProductSortingOptions options)
        {
            switch (options)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDescending(p => p.Name);
                    break;
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDescending(p => p.Price);
                    break;
            }
        }
    }
}
