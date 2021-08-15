namespace Scenario.Domain.SharedTypes
{
    public interface IDomainEvent<out TEntity> : IDomainEvent
        where TEntity : IScenarioEntity
    {
        TEntity Entity { get; }
    }

    public interface IDomainEvent
    {
        public long EntityId { get; }
    }
}
