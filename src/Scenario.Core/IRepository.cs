using System.Linq.Expressions;

namespace Scenario.Core;

public interface IRepository<T> : IQueryable<T>
    where T : class 
{
    Task<int> SaveAsync(CancellationToken cancellationToken);
    Task<int> AddAsync(T entity, CancellationToken cancellationToken);
    Task<int> AddRangeAsync(IEnumerable<T> entity, CancellationToken cancellationToken);
    
    ValueTask<T?> GetByIdAsync(Guid entityId, CancellationToken cancellationToken);
    ValueTask<IEnumerable<T>> GetAll(CancellationToken cancellationToken);
    ValueTask<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
    
    ValueTask DeleteAsync(Guid entityId, CancellationToken cancellationToken);
    ValueTask DeleteWhereAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
}