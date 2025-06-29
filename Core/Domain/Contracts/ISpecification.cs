using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Domain.Contracts
{
    public interface ISpecification<T> where T : class
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> IncludesExpressions { get; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; } // Fixed spelling: OrderByDecending -> OrderByDescending
        int Skip { get; }
        int Take { get; }
        bool IsPaginated { get; } // Fixed spelling: IsPagneated -> IsPaginated
    }
}