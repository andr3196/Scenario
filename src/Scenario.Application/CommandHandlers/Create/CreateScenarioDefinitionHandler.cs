using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Scenario.Contracts.Commands;

namespace Scenario.Application.CommandHandlers.Create
{
    public class CreateScenarioDefinitionHandler : IRequestHandler<CreateScenarioDefinition, Guid>
    {
        public Task<Guid> Handle(CreateScenarioDefinition request, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}