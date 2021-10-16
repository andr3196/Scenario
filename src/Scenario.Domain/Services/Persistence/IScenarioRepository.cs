using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Scenario.Domain.Models.Scenarios;

namespace Scenario.Domain.Services.Persistence
{
    public interface IScenarioRepository
    {
        Task<IEnumerable<ScenarioDefinition>> GetAllActiveScenarioDefinitionsAsync(CancellationToken cancellationToken);
    }
}
