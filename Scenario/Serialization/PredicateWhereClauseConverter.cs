using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scenario.Domain.Clauses;
using Scenario.Extensions;
using Scenario.Serialization.Extensions;

namespace Scenario.Serialization
{
    public class PredicateWhereClauseConverter : JsonConverter<IPredicateWhereClause>
    {
        public override IPredicateWhereClause Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var discriminator = reader.GetStringProperty("discriminator");
            if (discriminator == null)
            {
                throw new JsonException();
            }

            return (IPredicateWhereClause?)(discriminator switch
            {
                nameof(UnaryFilterWhereClause) => JsonSerializer.Deserialize<UnaryFilterWhereClause>(ref reader, options),
                nameof(BinaryFilterWhereClause) => JsonSerializer.Deserialize<BinaryFilterWhereClause>(ref reader, options),
                nameof(UnaryLogicalWhereClause) => JsonSerializer.Deserialize<UnaryLogicalWhereClause>(ref reader, options),
                nameof(BinaryLogicalWhereClause) => JsonSerializer.Deserialize<BinaryLogicalWhereClause>(ref reader, options),
                _ => throw new NotSupportedException()
            }) ?? throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, IPredicateWhereClause value, JsonSerializerOptions options)
        {
            if(value == null)
            {
                writer.WriteNullValue();
                return;
            } else if(value is UnaryFilterWhereClause unaryFilter)
            {
                JsonSerializer.Serialize(writer, unaryFilter, options);
                return;
            } else if(value is BinaryFilterWhereClause binaryFilter)
            {
                JsonSerializer.Serialize(writer, binaryFilter, options);
                return;
            } else if(value is UnaryLogicalWhereClause unary)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(nameof(UnaryLogicalWhereClause.Value).ToCamelCase());
                Write(writer, unary.Value, options);
                writer.WriteEndObject();
                return;
            } else if(value is BinaryLogicalWhereClause binary)
            {
                writer.WriteStartObject();
                writer.WriteString(nameof(BinaryLogicalWhereClause.Discriminator).ToCamelCase(), nameof(BinaryLogicalWhereClause));
                writer.WritePropertyName(nameof(BinaryLogicalWhereClause.Left).ToCamelCase());
                Write(writer, binary.Left, options);
                writer.WritePropertyName(nameof(BinaryLogicalWhereClause.Right).ToCamelCase());
                Write(writer, binary.Right, options);
                writer.WriteString(nameof(binary.Combinator).ToCamelCase(), binary.Combinator);
                writer.WriteEndObject();
                return;
            }
            throw new NotSupportedException($"Serialization of {value.GetType().Name} not supported");
        }
    }
}
