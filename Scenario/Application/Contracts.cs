using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Scenario.Domain.Clauses;
using Scenario.Serialization;

namespace Scenario.Application
{
    public record ScenarioConsequence(string Key, string ParametersType, Dictionary<string, ValueWhereClause> Parameters);

    public record ScenarioDefinitionDto
    {
        public string Entity { get; set; }

        public string Event { get; set; }

        public RootNodeWhereClause Condition { get; set; }

        public ScenarioConsequence Consequence { get; set; }
    }

    public record ScenarioDto(long Id, string Title, ScenarioDefinitionDto Scenario, DateTime Created, bool Active);

    public record ScenarioCreateDto(string Title, ScenarioDefinitionDto Scenario);

    public record ScenarioCreateResult(long Id, string Title, DateTime Created, bool Active);

}
