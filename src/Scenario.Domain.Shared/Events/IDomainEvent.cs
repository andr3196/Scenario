namespace Scenario.Domain.Shared.Events
{
    public interface IDomainEvent<out TEntity> : IDomainEvent
    {
        TEntity Entity { get; }
    }

    public interface IDomainEvent
    {
        public long EntityId { get; }
    }
}
