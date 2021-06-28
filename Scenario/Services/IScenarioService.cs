using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Scenario.Application;

namespace Scenario.Services
{
    public interface IScenarioService
    {
        Task<IEnumerable<ScenarioDto>> GetScenariosList(CancellationToken cancellationToken);

        Task<ScenarioCreateResult> Create(ScenarioCreateDto scenarioCreate, CancellationToken cancellationToken);

        Task Delete(string Id, CancellationToken cancellationToken);
    }
}
