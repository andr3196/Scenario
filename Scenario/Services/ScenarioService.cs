using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Scenario.Application;

namespace Scenario.Services
{
    public class ScenarioService : IScenarioService
    {
        public Task<ScenarioCreateResult> Create(ScenarioCreateDto scenarioCreate, CancellationToken cancellationToken)
        {
            return Task.FromResult(new ScenarioCreateResult
            (
                Id: "1",
                Title: "Title",
                Active: true,
                Created: DateTime.Now
            ));
        }

        public Task Delete(string Id, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task<IEnumerable<ScenarioDto>> GetScenariosList(CancellationToken cancellationToken)
        {
            IEnumerable<ScenarioDto> result = new List<ScenarioDto>
            {
                new ScenarioDto
                (
                    Id: "2",
                    Title: "Title",
                    Scenario: "WHEN activity created WHERE activity.Type equals 'Test' THEN send new Email",
                    Created: DateTime.Now.AddDays(1),
                    Active: true
                ),
                new ScenarioDto
                (
                    Id: "3",
                    Title: "Title 2",
                    Scenario: "WHEN case created THEN SET case.",
                    Created: DateTime.Now.AddDays(1),
                    Active: true
                ),
            };
            return Task.FromResult(result);
        }
    }
}
