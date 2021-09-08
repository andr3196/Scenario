using System;
using Scenario.Application;
using Scenario.Domain.ScenarioDefinitions;

namespace Scenario.Services
{
    public interface IScenarioParsingService
    {
        public bool TryParse(ScenarioDefinitionDto scenario, out ScenarioDefinition? scenarioDefinition);

        public bool TryParse(ScenarioDefinitionDto scenario, out ScenarioDefinition? scenarioDefinition, out Exception? exception);

        public ScenarioDefinition Parse(ScenarioDefinitionDto scenario);
    }
}
