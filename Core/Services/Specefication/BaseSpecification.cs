using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Services.Specefication
{
    public abstract class BaseSpecification<T> : ISpecification<T> where T : class
    {
        protected BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        protected BaseSpecification()
        {
            // Empty constructor for cases where criteria isn't needed
        }

        public Expression<Func<T, bool>> Criteria { get; private set; }
        public List<Expression<Func<T, object>>> IncludesExpressions { get; } = [];
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; } // Fixed spelling: Decending -> Descending
        public int Skip { get; private set; }
        public int Take { get; private set; }
        public bool IsPaginated { get; private set; } // Fixed spelling: IsPagenated -> IsPaginated

        protected void ApplyPagination(int pageSize, int pageIndex) // Fixed spelling: ApplyPagenation -> ApplyPagination
        {
            IsPaginated = true; // Using only one property for pagination
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize; // 0 based
        }

        protected void AddInclude(Expression<Func<T, object>> includeExpression) =>
            IncludesExpressions.Add(includeExpression);

        protected void AddOrderBy(Expression<Func<T, object>> orderBy) // Fixed variable name: orderby -> orderBy
        {
            OrderBy = orderBy;
        }

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDesc) // Fixed spelling: AddOrderByDecnding -> AddOrderByDescending
        {
            OrderByDescending = orderByDesc; // Fixed property name to match: OrderByDecending -> OrderByDescending
        }
    }
}