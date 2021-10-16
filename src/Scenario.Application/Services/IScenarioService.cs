using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Scenario.Application.Models;

namespace Scenario.Application.Services
{
    public interface IScenarioService
    {
        Task<IEnumerable<ScenarioDto>> GetScenariosList(CancellationToken cancellationToken);

        Task<ScenarioDto> GetScenario(long id, CancellationToken cancellationToken);

        Task<ScenarioCreateResult> Create(ScenarioCreateDto scenarioCreate, CancellationToken cancellationToken);

        Task<ScenarioUpdateResult> Update(ScenarioUpdateDto scenarioCreate, CancellationToken cancellationToken);

        Task Delete(long id, CancellationToken cancellationToken);
    }
}
