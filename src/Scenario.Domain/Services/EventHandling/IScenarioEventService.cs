using System.Threading;
using System.Threading.Tasks;
using Scenario.Domain.Models.Scenarios;
using Scenario.Domain.Shared.Events;

namespace Scenario.Domain.Services.EventHandling
{
    public interface IScenarioEventService
    {
        Task HandleEvent<TEvent, TEntity>(TEvent @event, CancellationToken cancellationToken)
            where TEvent : IDomainEvent<TEntity>
            where TEntity : class;

        Task LoadAllAsync(CancellationToken cancellationToken);

        Task WaitForTasksToCompleteAsync(CancellationToken cancellationToken);
    }
}
