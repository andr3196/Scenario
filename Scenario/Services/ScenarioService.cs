using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Scenario.Application;
using Scenario.Infrastructure;

namespace Scenario.Services
{
    public class ScenarioService : IScenarioService
    {
        private readonly IDatabaseContext databaseContext;
        private readonly IScenarioParsingService scenarioParsingService;

        public ScenarioService(IDatabaseContext databaseContext, IScenarioParsingService scenarioParsingService)
        {
            this.databaseContext = databaseContext;
            this.scenarioParsingService = scenarioParsingService;
        }

        public async Task<ScenarioCreateResult> Create(ScenarioCreateDto scenarioCreate, CancellationToken cancellationToken)
        {
            var isValid = scenarioParsingService.TryParse(scenarioCreate.Scenario, out var scenarioDefinition);
            if (!isValid)
            {
                throw new ArgumentException("The scenario is invalid.");
            }

            var scenario = new Domain.Scenarios.Scenario
            {
                Active = true,
                Created = DateTime.Now,
                Description = JsonSerializer.Serialize(scenarioCreate.Scenario),
                Title = scenarioCreate.Title,
            };

            databaseContext.Set<Domain.Scenarios.Scenario>().Add(scenario);

            await databaseContext.SaveChangesAsync(cancellationToken);
            

            return new ScenarioCreateResult
            (
                Active: scenario.Active,
                Created: scenario.Created,
                Id: scenario.Id,
                Title: scenario.Title
            );
        }

        public async Task Delete(long id, CancellationToken cancellationToken)
        {
            var scenario = await databaseContext.Set<Domain.Scenarios.Scenario>()
                .SingleOrDefaultAsync(s => s.Id == id);
            if(scenario != null)
            {
                databaseContext.Set<Domain.Scenarios.Scenario>().Remove(scenario);
            }
            await databaseContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<ScenarioDto>> GetScenariosList(CancellationToken cancellationToken)
        {
            return (await databaseContext.Set<Domain.Scenarios.Scenario>()
                .Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.Description,
                    s.Created,
                    s.Active
                }).ToListAsync(cancellationToken))
                .Select( x => new ScenarioDto
                (
                    x.Id,
                    x.Title,
                    JsonSerializer.Deserialize<ScenarioDefinitionDto>(x.Description),
                    x.Created,
                    x.Active
                    ))
                .ToList();
        }
    }
}
