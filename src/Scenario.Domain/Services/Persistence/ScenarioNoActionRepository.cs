using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Scenario.Domain.Models.Scenarios;

namespace Scenario.Domain.Services.Persistence
{
    public class ScenarioNoActionRepository : IScenarioRepository
    {
        public ScenarioNoActionRepository()
        {
        }

        public Task<IEnumerable<ScenarioDefinition>> GetAllActiveScenarioDefinitionsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(new List<ScenarioDefinition>().AsEnumerable());
        }
    }
}
