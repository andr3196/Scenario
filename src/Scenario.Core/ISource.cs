namespace Scenario.Core;

public interface ISource<out TEntity> : IQueryable<TEntity>
{
    
}