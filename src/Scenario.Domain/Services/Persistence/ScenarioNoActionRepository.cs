using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Scenario.Core.Persistence;
using Scenario.Domain.Models;

namespace Scenario.Domain.Services.Persistence
{
    public class ScenarioNoActionRepository : IScenarioRepository
    {
        public ScenarioNoActionRepository()
        {
        }

        public Task<IEnumerable<ScenarioFlow>> GetAllActiveScenarioDefinitionsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(new List<ScenarioFlow>().AsEnumerable());
        }
    }
}
