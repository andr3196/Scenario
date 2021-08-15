using System.Collections.Generic;

namespace Scenario.Domain.SharedTypes
{
    public interface IScenarioDataProvider
    {
        object GetData<TEvent, TEntity>(TEvent @event, IEnumerable<string> includes = null)
            where TEvent : IDomainEvent<TEntity>
            where TEntity : class, IScenarioEntity;
    }
}
