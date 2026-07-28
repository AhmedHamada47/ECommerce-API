using Domain.Contracts;
using System.Linq.Expressions;

namespace Services.Specification
{
    public abstract class BaseSpecification<T> : ISpecification<T> where T : class
    {
        protected BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        protected BaseSpecification()
        {
        }

        public Expression<Func<T, bool>> Criteria { get; private set; }
        public List<Expression<Func<T, object>>> IncludesExpressions { get; } = [];
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }
        public int Skip { get; private set; }
        public int Take { get; private set; }
        public bool IsPaginated { get; private set; }

        protected void ApplyPagination(int pageSize, int pageIndex)
        {
            IsPaginated = true;
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize;
        }

        protected void AddInclude(Expression<Func<T, object>> includeExpression) =>
            IncludesExpressions.Add(includeExpression);

        protected void AddOrderBy(Expression<Func<T, object>> orderBy)
        {
            OrderBy = orderBy;
        }

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDesc)
        {
            OrderByDescending = orderByDesc;
        }
    }
}
