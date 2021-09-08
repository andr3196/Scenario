using System;
namespace Scenario.Domain.ScenarioDefinitions
{
    public class ScenarioDefinitionBuilder
    {
        private readonly Type entity;

        public ScenarioDefinitionBuilder(Type entity)
        {
            this.entity = entity;
        }
    }
}
