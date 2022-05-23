using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Scenario.Domain.Models;

namespace Scenario.Core.Persistence
{
    public interface IScenarioRepository
    {
        Task<IEnumerable<ScenarioFlow>> GetAllActiveScenarioDefinitionsAsync(CancellationToken cancellationToken);
    }
}
