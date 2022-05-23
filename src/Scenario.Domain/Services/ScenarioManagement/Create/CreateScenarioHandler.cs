using System.Threading;
using System.Threading.Tasks;
using Scenario.Core;
using Scenario.Domain.Services.ScenarioManagement.DataAccess;

namespace Scenario.Domain.Services.ScenarioManagement.Create;

public class CreateScenarioHandler  : ICommandHandler<CreateScenario>
{
    private readonly IScenarioRepository scenarioRepository;

    public CreateScenarioHandler(IScenarioRepository scenarioRepository)
    {
        this.scenarioRepository = scenarioRepository;
    }
    
    public ValueTask Handle(CreateScenario command, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }
}