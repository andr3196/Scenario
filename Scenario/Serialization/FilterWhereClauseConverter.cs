using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scenario.Domain.Clauses;
using Scenario.Domain.Modeling.Models.Filters;
using Scenario.Serialization.Extensions;

namespace Scenario.Serialization
{
    public class FilterWhereClauseConverter : JsonConverter<FilterWhereClause>
    {
        private readonly IEnumerable<IFilter> filters;

        public FilterWhereClauseConverter(IEnumerable<IFilter> filters)
        {
            this.filters = filters;
        }

        public override FilterWhereClause Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Utf8JsonReader readerClone = reader;

            var discriminator = reader.GetStringProperty("discriminator");
            if (discriminator == null)
            {
                throw new JsonException();
            }

            FilterWhereClause clause = discriminator switch
            {
                nameof(UnaryFilterWhereClause) => new UnaryFilterWhereClause(),
                nameof(BinaryFilterWhereClause) => new BinaryFilterWhereClause(filters),
                _ => throw new NotSupportedException()
            };

            return (FilterWhereClause)(JsonSerializer.Deserialize(ref reader, clause.GetType()) ?? throw new JsonException());
        }

        public override void Write(Utf8JsonWriter writer, FilterWhereClause value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
