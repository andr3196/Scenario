using Scenario.Domain.Shared.Events;

namespace Scenario.Events
{
    public interface IScenarioEventService
    {
        Task HandleEvent<TEvent, TEntity>(TEvent @event, CancellationToken cancellationToken)
            where TEvent : IDomainEvent<TEntity>
            where TEntity : class;

        Task LoadAllAsync(CancellationToken cancellationToken);
    }
}
