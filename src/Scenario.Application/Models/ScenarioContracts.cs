using System;
using Scenario.Domain.Clauses;

namespace Scenario.Application.Models
{
    public record ScenarioDefinitionDto
    {
        public string Entity { get; set; }

        public string Event { get; set; }

        public IPredicateClause Condition { get; set; }

        public ConsequenceClause Consequence { get; set; }
    }

    public record ScenarioDto(long Id, string Title, ScenarioDefinitionDto Scenario, DateTime Created, bool Active);

    public record ScenarioCreateDto(string Title, ScenarioDefinitionDto Scenario);

    public record ScenarioCreateResult(long Id, string Title, DateTime Created, bool Active);

    public record ScenarioUpdateDto(long Id, ScenarioDefinitionDto Scenario);

    public record ScenarioUpdateResult(long Id, string Title, DateTime Updated, bool Active);
}
