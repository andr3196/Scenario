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
using Scenario.Serialization;

namespace Scenario.Services
{
    public class ScenarioService : IScenarioService
    {
        private readonly IDatabaseContext databaseContext;
        private readonly IScenarioParsingService scenarioParsingService;
        private readonly IScenarioEventService scenarioEventService;
        private readonly ISerializationService serializationService;

        public ScenarioService(
            IDatabaseContext databaseContext,
            IScenarioParsingService scenarioParsingService,
            IScenarioEventService scenarioEventService,
            ISerializationService serializationService)
        {
            this.databaseContext = databaseContext;
            this.scenarioParsingService = scenarioParsingService;
            this.scenarioEventService = scenarioEventService;
            this.serializationService = serializationService;
        }

        public async Task<ScenarioCreateResult> Create(ScenarioCreateDto scenarioCreate, CancellationToken cancellationToken)
        {
            var isValid = scenarioParsingService.TryParse(scenarioCreate.Scenario, out var scenarioDefinition, out var parsingException);
            if (!isValid)
            {
                throw parsingException!;
            }

            var description = serializationService.Serialize(scenarioCreate.Scenario);

            var scenario = new Domain.Scenarios.Scenario
            {
                Active = true,
                Created = DateTime.Now,
                Description = description,
                Title = scenarioCreate.Title,
            };

            databaseContext.Set<Domain.Scenarios.Scenario>().Add(scenario);

            await databaseContext.SaveChangesAsync(cancellationToken);

            scenarioEventService.Register(scenarioDefinition!);

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
