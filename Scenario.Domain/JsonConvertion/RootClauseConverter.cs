using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scenario.Domain.Clauses;
using Scenario.Domain.JsonConvertion.Extensions;
using Scenario.Domain.Extensions;

namespace Scenario.Domain.JsonConvertion
{
    public class RootClauseConverter : JsonConverter<RootClause>
    {
        private const string DiscriminatorPropertyName = "Discriminator";

        public override RootClause Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var discriminatorPropertyNameInCase = options.PropertyNamingPolicy == JsonNamingPolicy.CamelCase
                ? DiscriminatorPropertyName.ToCamelCase() : DiscriminatorPropertyName;
                
            var discriminator = reader.GetStringProperty(discriminatorPropertyNameInCase);
            if(discriminator == null)
            {
                throw new JsonException();
            }
            var type = discriminator switch
            {
                nameof(UnaryPredicateClause) => typeof(UnaryPredicateClause),
                nameof(BinaryPredicateClause) => typeof(BinaryPredicateClause),
                nameof(UnaryOperatorClause) => typeof(UnaryOperatorClause),
                nameof(BinaryOperatorClause) => typeof(BinaryOperatorClause),
                _ => throw new JsonException()
            };
            var predicate = JsonSerializer.Deserialize(ref reader, type, options) as IPredicateClause ?? throw new JsonException();
            return new RootClause(predicate);
        }


        public override void Write(Utf8JsonWriter writer, RootClause value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(value.Value));
            JsonSerializer.Serialize(writer, value.Value, options);
            writer.WriteEndObject();
        }
    }
}
