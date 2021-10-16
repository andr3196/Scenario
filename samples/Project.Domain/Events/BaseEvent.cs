using Scenario.Domain.SharedTypes;

namespace Project.Domain.Events
{
    public abstract class BaseEvent<TEntity> : IDomainEvent<TEntity>
        where TEntity : IScenarioEntity
    {
        public BaseEvent(TEntity entity)
        {
            Entity = entity;
        }

        public long EntityId => Entity.Id;
        public TEntity Entity { get; }
    }
}
