using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scenario.Domain.Clauses;
using Scenario.Domain.Extensions;
using Scenario.Domain.JsonConvertion.Extensions;

namespace Scenario.Domain.JsonConvertion
{
    public class PredicateClauseConverter : JsonConverter<IPredicateClause>
    {
        private const string DiscriminatorPropertyName = "Discriminator";

        public override IPredicateClause Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var discriminatorPropertyNameInCase = options.PropertyNamingPolicy == JsonNamingPolicy.CamelCase
                ? DiscriminatorPropertyName.ToCamelCase() : DiscriminatorPropertyName;
            
            var discriminator = reader.GetStringProperty(discriminatorPropertyNameInCase);
            if (discriminator == null)
            {
                throw new JsonException();
            }

            return (IPredicateClause?)(discriminator switch
            {
                nameof(UnaryOperatorClause) => JsonSerializer.Deserialize<UnaryOperatorClause>(ref reader, options),
                nameof(BinaryOperatorClause) => JsonSerializer.Deserialize<BinaryOperatorClause>(ref reader, options),
                nameof(UnaryPredicateClause) => JsonSerializer.Deserialize<UnaryPredicateClause>(ref reader, options),
                nameof(BinaryPredicateClause) => JsonSerializer.Deserialize<BinaryPredicateClause>(ref reader, options),
                _ => throw new NotSupportedException()
            }) ?? throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, IPredicateClause value, JsonSerializerOptions options)
        {
            if(value == null)
            {
                writer.WriteNullValue();
                return;
            } else if(value is UnaryOperatorClause unaryFilter)
            {
                JsonSerializer.Serialize(writer, unaryFilter, options);
                return;
            } else if(value is BinaryOperatorClause binaryFilter)
            {
                JsonSerializer.Serialize(writer, binaryFilter, options);
                return;
            } else if(value is UnaryPredicateClause unary)
            {
                writer.WriteStartObject();
                writer.WriteString(nameof(UnaryPredicateClause.Discriminator).ToCamelCase(), nameof(UnaryPredicateClause));
                writer.WritePropertyName(nameof(UnaryPredicateClause.Value).ToCamelCase());
                Write(writer, unary.Value, options);
                writer.WriteEndObject();
                return;
            } else if(value is BinaryPredicateClause binary)
            {
                writer.WriteStartObject();
                writer.WriteString(nameof(BinaryPredicateClause.Discriminator).ToCamelCase(), nameof(BinaryPredicateClause));
                writer.WriteString(nameof(binary.Combinator).ToCamelCase(), binary.Combinator);
                writer.WritePropertyName(nameof(BinaryPredicateClause.Left).ToCamelCase());
                Write(writer, binary.Left, options);
                writer.WritePropertyName(nameof(BinaryPredicateClause.Right).ToCamelCase());
                Write(writer, binary.Right, options);
                writer.WriteEndObject();
                return;
            }
            throw new NotSupportedException($"Serialization of {value.GetType().Name} not supported");
        }
    }
}
