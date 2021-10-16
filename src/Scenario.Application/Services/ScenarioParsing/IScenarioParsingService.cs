using System;
using Scenario.Application.Models;
using Scenario.Domain.Models.Scenarios;

namespace Scenario.Application.Services.ScenarioParsing
{
    public interface IScenarioParsingService
    {
        public bool TryParse(ScenarioDefinitionDto scenario, out ScenarioDefinition? scenarioDefinition);

        public bool TryParse(ScenarioDefinitionDto scenario, out ScenarioDefinition? scenarioDefinition, out Exception? exception);

        public ScenarioDefinition Parse(ScenarioDefinitionDto scenario);
    }
}
