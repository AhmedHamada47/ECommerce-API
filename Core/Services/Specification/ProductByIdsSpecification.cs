using Domain.Models;
using System.Linq.Expressions;

namespace Services.Specification
{
    internal class ProductByIdsSpecification : BaseSpecification<Product>
    {
        public ProductByIdsSpecification(IEnumerable<int> ids)
            : base(CreateCriteria(ids))
        {
        }

        private static Expression<Func<Product, bool>> CreateCriteria(IEnumerable<int> ids)
        {
            var idSet = ids.ToHashSet();
            return product => idSet.Contains(product.Id);
        }
    }
}
