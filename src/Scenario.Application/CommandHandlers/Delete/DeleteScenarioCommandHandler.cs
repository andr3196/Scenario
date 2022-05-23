using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Scenario.Contracts.Commands;
using Scenario.Core;
using Scenario.Domain.Models;

namespace Scenario.Application.CommandHandlers.Delete;

public class DeleteScenarioCommandHandler : IDeleteScenarioCommandHandler
{
    private readonly IRepository<ScenarioDefinition> scenarios;

    public DeleteScenarioCommandHandler(IRepository<ScenarioDefinition> scenarios)
    {
        this.scenarios = scenarios;
    }
    
    public async Task<Unit> Handle(DeleteScenarioCommand request, CancellationToken cancellationToken)
    {
        await scenarios.DeleteAsync(request.Id, cancellationToken);
        return new Unit();
    }
}