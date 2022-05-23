using System;
using System.Threading;
using System.Threading.Tasks;
using Scenario.Contracts.Commands;

namespace Scenario.Application.CommandHandlers.Update;

public class UpdateScenarioCommandHandler : IUpdateScenarioCommandHandler
{
    public Task<Guid> Handle(UpdateScenarioCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}