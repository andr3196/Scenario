using System.Text.Json;
using Scenario.Domain.JsonConvertion;

namespace Scenario.Domain
{
    public class ScenarioSerializer : IScenarioSerializer
    {
        private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();

        public ScenarioSerializer()
        {
            jsonSerializerOptions.Converters.Add(new RootClauseConverter());
            //jsonSerializerOptions.Converters.Add(new PredicateWhereClauseConverter());
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
