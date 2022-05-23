using System.Text.Json;
using Scenario.Domain.Serialization.JsonConvertion;

namespace Scenario.Domain.Serialization
{
    public class ScenarioSerializer : IScenarioSerializer
    {
        private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();

        public ScenarioSerializer()
        {
            jsonSerializerOptions.Converters.Add(new PredicateClauseConverter());
            jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        }

        public TType? Deserialize<TType>(string type)
        {
            return JsonSerializer.Deserialize<TType>(type, jsonSerializerOptions);
        }

        public string Serialize<TType>(TType type)
        {
            return JsonSerializer.Serialize(type, jsonSerializerOptions);
        }
    }
}
