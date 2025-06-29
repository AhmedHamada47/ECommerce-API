using Shared.Dto.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specefication
{
    internal class ProductWithBrandAndTypeSpecifications
        : BaseSpecification<Product>
    {
        // Use this CTOR to Create Query to Get Product By Id  
        public ProductWithBrandAndTypeSpecifications(int id)
            : base(criteria: product => product.Id == id)
        {
            //Add includes
            AddInclude(product => product.ProductBrand);
            AddInclude(product => product.ProductType);
        }
        //for for sorting & filteration

        public ProductWithBrandAndTypeSpecifications(ProductQueryParameter parameter) : base(CreateCrateria(parameter))
        {
            AddInclude(product => product.ProductBrand);
            AddInclude(product => product.ProductType);
            ApplySorting(parameter.options);
            ApplyPagination(parameter.PageSize, parameter.PageIndex);

        }

        private static Expression<Func<Product, bool>> CreateCrateria(ProductQueryParameter parameter)
        {
            return product =>
                 (!parameter.BrandId.HasValue || product.BrandId == parameter.BrandId.Value) &&
                 (!parameter.Typeid.HasValue || product.TypeId == parameter.Typeid.Value) &&
                 (string.IsNullOrWhiteSpace(parameter.Search) || product.Name.ToLower().Contains(parameter.Search.ToLower())); ;
        }

        private void ApplySorting(ProductSortinOptions options)
        {
            switch (options) {
                case ProductSortinOptions.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;
                case ProductSortinOptions.NameDesc:
                    AddOrderByDescending(p => p.Name);
                    break;
                case ProductSortinOptions.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;
                case ProductSortinOptions.PriceDesc:
                    AddOrderByDescending(p => p.Price);
                    break;

                default:
                    break;
            }
        }
    }

}