using Constellation.Bca.Domain.Entites;
using Constellation.Bca.Domain.ValueObjects;
using System.Linq.Expressions;

namespace Constellation.Bca.Domain.Interfaces.Services
{
    public interface IQueryFilterService<T> where T : class
    {
        /// <summary>
        /// Creates an <see cref="Expression{T}"/> where the delegate type is known at compile time.
        /// </summary>
        /// <returns>An <see cref="Expression{T}.</returns>
        Expression<Func<T, bool>> GetExpressionFilter(QueryFilter queryFilter);
    }
}
