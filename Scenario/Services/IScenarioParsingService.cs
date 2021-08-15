using System;
using Scenario.Application;
using Scenario.Domain.ScenarioDefinitions;

namespace Scenario.Services
{
    public interface IScenarioParsingService
    {
        public bool TryParse(ScenarioDefinitionDto scenario, out ScenarioDefinition scenarioDefinition);

        public ScenarioDefinition Parse(ScenarioDefinitionDto scenario);
    }
}
