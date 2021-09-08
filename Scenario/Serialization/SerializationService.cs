using System;
using System.Text.Json;

namespace Scenario.Serialization
{
    public class SerializationService : ISerializationService
    {
        private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();

        public SerializationService()
        {
            jsonSerializerOptions.Converters.Add(new RootWhereClauseConverter());
            jsonSerializerOptions.Converters.Add(new PredicateWhereClauseConverter());
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
