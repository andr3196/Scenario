using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Scenario.Core;

namespace Scenario.EntityFrameworkCore;

public class Repository<T> : IRepository<T>
    where T : class
{
    private readonly DatabaseContext dbContext;
    private readonly IQueryable<T> baseQueryable;

    public Repository(DatabaseContext dbContext)
    {
        this.dbContext = dbContext;
        baseQueryable = dbContext.Set<T>();
        dbContext.Database.EnsureCreated();
    }
    
    public Task<int> AddAsync(T entity, CancellationToken cancellationToken)
    {
        dbContext.Set<T>().Add(entity);
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<int> AddRangeAsync(IEnumerable<T> entites, CancellationToken cancellationToken)
    {
        dbContext.Set<T>().AddRange(entites);
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public ValueTask<T?> GetByIdAsync(Guid entityId, CancellationToken cancellationToken)
    {
        return dbContext.Set<T>().FindAsync(new object?[] { entityId }, cancellationToken: cancellationToken);
    }

    public async ValueTask<IEnumerable<T>> GetAll(CancellationToken cancellationToken)
    {
        return await dbContext.Set<T>().ToListAsync(cancellationToken);
    }

    public async ValueTask<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
    {
        return await dbContext.Set<T>().Where(predicate).ToListAsync(cancellationToken);
    }

    public async ValueTask DeleteAsync(Guid entityId, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Set<T>().FindAsync(new object?[] { entityId, cancellationToken }, cancellationToken)
                     ?? throw new InvalidOperationException();
        dbContext.Set<T>().Remove(entity);
    }

    public async ValueTask DeleteWhereAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
    {
        var entities = await dbContext.Set<T>().Where(predicate).ToListAsync(cancellationToken);
        dbContext.Set<T>().RemoveRange(entities);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return baseQueryable.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public Type ElementType => baseQueryable.ElementType;
    public Expression Expression => baseQueryable.Expression;
    public IQueryProvider Provider => baseQueryable.Provider;
}