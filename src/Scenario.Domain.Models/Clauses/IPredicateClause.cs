using System.Text.Json.Serialization;

namespace Scenario.Domain.Models.Clauses
{
    public interface IPredicateClause
    {
        [JsonPropertyOrder(-1)]
        public string Discriminator { get; }
    }
}
