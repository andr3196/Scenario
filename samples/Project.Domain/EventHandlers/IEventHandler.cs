using System.Threading;
using System.Threading.Tasks;
using Scenario.Domain.Shared.Events;

namespace Project.Domain.EventHandlers
{
    public interface IEventHandler<in TEvent> where TEvent : IDomainEvent
    {

        Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
    }
}
