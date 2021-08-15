using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scenario.Domain.Clauses;
using Scenario.Domain.Modeling.Models.Filters;

namespace Scenario.Serialization
{
    public class ValueWhereClauseConverter : JsonConverter<ValueWhereClause>
    {
        private readonly IEnumerable<IFilter> filters;

        public ValueWhereClauseConverter(IEnumerable<IFilter> filters)
        {
            this.filters = filters;
        }

        public override ValueWhereClause Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Utf8JsonReader readerClone = reader;

            if (readerClone.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            readerClone.Read();
            if (readerClone.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            string propertyName = readerClone.GetString();

            FilterWhereClause clause = propertyName switch
            {
                "Value" => new UnaryFilterWhereClause(),
                "Left" => new BinaryFilterWhereClause(filters),
                _ => throw new NotSupportedException()
            };

            return null; // (FilterWhereClause)JsonSerializer.Deserialize(ref reader, clause.GetType());
        }

        public override void Write(Utf8JsonWriter writer, ValueWhereClause value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
