using System;
using System.Threading.Tasks;
using Scenario.Domain.SharedTypes;

namespace Project.Domain.EventHandlers
{
    public interface IEventHandler<in TEvent> where TEvent : IDomainEvent
    {

        Task HandleAsync(TEvent @event);
    }
}
