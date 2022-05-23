using System.Collections;
using System.Linq.Expressions;
using Scenario.Core;

namespace Scenario.EntityFrameworkCore;

public class Source<TEntity> : ISource<TEntity> where TEntity : class
{
    private readonly IQueryable<TEntity> baseQueryable; 

    public Source(DatabaseContext context)
    {
        context.Database.EnsureCreated();
        baseQueryable = context.Set<TEntity>();
    }
    
    public IEnumerator<TEntity> GetEnumerator()
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