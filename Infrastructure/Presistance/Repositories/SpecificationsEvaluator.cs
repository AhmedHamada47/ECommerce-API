using Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Presistance.Repositories
{
    internal static class SpecificationsEvaluator
    {
        public static IQueryable<T> CreateQuery<T>(IQueryable<T> inputQuery, ISpecification<T> specification)
            where T : class
        {
            var q = inputQuery;
            if (specification.Criteria != null)
            {
                q = q.Where(specification.Criteria);
            }

            q = specification.IncludesExpressions.Aggregate(q,
                (currentQuery, include) => currentQuery.Include(include));

            if (specification.OrderBy is not null)
            {
                q = q.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending is not null)
            {
                q = q.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.IsPaginated)
            {
                q = q.Skip(specification.Skip).Take(specification.Take);
            }

            return q;
        }
    }
}
