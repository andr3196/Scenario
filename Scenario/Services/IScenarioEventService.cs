using System;
using System.Threading;
using System.Threading.Tasks;
using Scenario.Domain;
using Scenario.Domain.SharedTypes;

namespace Scenario.Services
{
    public interface IScenarioEventService
    {
        Task HandleEvent<TEvent, TEntity>(TEvent @event, CancellationToken cancellationToken)
            where TEvent : IDomainEvent<TEntity>
            where TEntity : class, IScenarioEntity;
    }
}
