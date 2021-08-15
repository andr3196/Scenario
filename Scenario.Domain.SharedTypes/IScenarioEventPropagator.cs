using System;
using System.Threading.Tasks;

namespace Scenario.Domain.SharedTypes
{
    public interface IScenarioEventPropagator
    {
        public Task PropagateAsync<TEvent, TEntity>(TEvent @event)
            where TEvent : IDomainEvent<TEntity>
            where TEntity : class, IScenarioEntity;
    }
}
