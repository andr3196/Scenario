using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Scenario.Application.Models;

namespace Scenario.Application.Services
{
    public class ScenarioService : IScenarioService
    {
        public ScenarioService()
        {
        }

        public Task<ScenarioCreateResult> Create(ScenarioCreateDto scenarioCreate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Delete(long id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ScenarioDto> GetScenario(long id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ScenarioDto>> GetScenariosList(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<ScenarioUpdateResult> Update(ScenarioUpdateDto scenarioCreate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
