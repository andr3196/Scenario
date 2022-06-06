using System;
using System.Text.Json;
using Scenario.Domain.Models.Clauses;

namespace Scenario.Domain.Serialization.JsonConvertion
{
    public class ConsequenceClauseConverter : IConsequenceClauseConverter
    {
        private readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new PredicateClauseConverter() },
        };
        
        public string Serialize(ConsequenceClause clause)
        {
            return JsonSerializer.Serialize(clause, options);
        }

        public ConsequenceClause Deserialize(string clauseJson)
        {
            return JsonSerializer.Deserialize<ConsequenceClause>(clauseJson, options) ?? throw new InvalidOperationException();
        }
    }
}