using System;
using System.Text.Json;
using Scenario.Domain.Models.Clauses;

namespace Scenario.Domain.Serialization.JsonConvertion
{
    public class ConsequenceClauseConverter : IConsequenceClauseConverter
    {
        public string Serialize(ConsequenceClause clause)
        {
            return JsonSerializer.Serialize(clause);
        }

        public ConsequenceClause Deserialize(string clauseJson)
        {
            return JsonSerializer.Deserialize<ConsequenceClause>(clauseJson) ?? throw new InvalidOperationException();
        }
    }
}