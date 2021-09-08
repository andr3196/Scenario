using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scenario.Domain.Clauses;
using Scenario.Serialization.Extensions;

namespace Scenario.Serialization
{
    public class RootWhereClauseConverter : JsonConverter<RootNodeWhereClause>
    {
        public RootWhereClauseConverter()
        {
        }

        public override RootNodeWhereClause Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var discriminator = reader.GetStringProperty("discriminator");
            if(discriminator == null)
            {
                throw new JsonException();
            }
            var type = discriminator switch
            {
                nameof(UnaryLogicalWhereClause) => typeof(UnaryLogicalWhereClause),
                nameof(BinaryLogicalWhereClause) => typeof(BinaryLogicalWhereClause),
                nameof(UnaryFilterWhereClause) => typeof(UnaryFilterWhereClause),
                nameof(BinaryFilterWhereClause) => typeof(BinaryFilterWhereClause),
                _ => throw new JsonException()
            };
            var predicate = JsonSerializer.Deserialize(ref reader, type, options) ?? throw new JsonException();
            return new RootNodeWhereClause
            {
                Value = predicate as IPredicateWhereClause ?? throw new JsonException(),
            };
        }


        public override void Write(Utf8JsonWriter writer, RootNodeWhereClause value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(value.Value));
            JsonSerializer.Serialize(writer, value.Value, options);
            writer.WriteEndObject();
        }
    }
}
