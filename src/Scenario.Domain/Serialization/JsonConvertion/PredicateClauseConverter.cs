using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scenario.Domain.Extensions;
using Scenario.Domain.Models.Clauses;
using Scenario.Domain.Serialization.JsonConvertion.Extensions;

namespace Scenario.Domain.Serialization.JsonConvertion
{
    public class PredicateClauseConverter : JsonConverter<IPredicateClause>, IPredicateClauseConverter
    {
        private JsonSerializerOptions? options;


        private JsonSerializerOptions Options
        {
            get
            {
                return options ??= new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Converters = {this},
                };
            }
        }

        private const string DiscriminatorPropertyName = "Discriminator";

        public override IPredicateClause Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions serializerOptions)
        {
            var discriminatorPropertyNameInCase = serializerOptions.PropertyNamingPolicy == JsonNamingPolicy.CamelCase
                ? DiscriminatorPropertyName.ToCamelCase() : DiscriminatorPropertyName;
            
            var discriminator = reader.GetStringProperty(discriminatorPropertyNameInCase);
            if (discriminator == null)
            {
                throw new JsonException();
            }

            return (IPredicateClause?)(discriminator switch
            {
                nameof(UnaryOperatorClause) => JsonSerializer.Deserialize<UnaryOperatorClause>(ref reader, serializerOptions),
                nameof(BinaryOperatorClause) => JsonSerializer.Deserialize<BinaryOperatorClause>(ref reader, serializerOptions),
                nameof(UnaryPredicateClause) => JsonSerializer.Deserialize<UnaryPredicateClause>(ref reader, serializerOptions),
                nameof(BinaryPredicateClause) => JsonSerializer.Deserialize<BinaryPredicateClause>(ref reader, serializerOptions),
                _ => throw new NotSupportedException()
            }) ?? throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, IPredicateClause value, JsonSerializerOptions serializerOptions)
        {
            switch (value)
            {
                case null:
                    writer.WriteNullValue();
                    return;
                case UnaryOperatorClause unaryFilter:
                    JsonSerializer.Serialize(writer, unaryFilter, serializerOptions);
                    return;
                case BinaryOperatorClause binaryFilter:
                    JsonSerializer.Serialize(writer, binaryFilter, serializerOptions);
                    return;
                case UnaryPredicateClause unary:
                    writer.WriteStartObject();
                    writer.WriteString(nameof(UnaryPredicateClause.Discriminator).ToCamelCase(), nameof(UnaryPredicateClause));
                    writer.WritePropertyName(nameof(UnaryPredicateClause.Value).ToCamelCase());
                    Write(writer, unary.Value, serializerOptions);
                    writer.WriteEndObject();
                    return;
                case BinaryPredicateClause binary:
                    writer.WriteStartObject();
                    writer.WriteString(nameof(BinaryPredicateClause.Discriminator).ToCamelCase(), nameof(BinaryPredicateClause));
                    writer.WriteString(nameof(binary.Combinator).ToCamelCase(), binary.Combinator);
                    writer.WritePropertyName(nameof(BinaryPredicateClause.Left).ToCamelCase());
                    Write(writer, binary.Left, serializerOptions);
                    writer.WritePropertyName(nameof(BinaryPredicateClause.Right).ToCamelCase());
                    Write(writer, binary.Right, serializerOptions);
                    writer.WriteEndObject();
                    return;
                default:
                    throw new NotSupportedException($"Serialization of {value.GetType().Name} not supported");
            }
        }

        public string Serialize(IPredicateClause clause)
        {
            return JsonSerializer.Serialize(clause, Options);
        }

        public IPredicateClause Deserialize(string clauseJson)
        {
            return JsonSerializer.Deserialize<IPredicateClause>(clauseJson) ?? throw new InvalidOperationException();
        }
    }
}
