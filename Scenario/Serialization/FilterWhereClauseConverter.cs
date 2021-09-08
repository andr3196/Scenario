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
            var discriminator = reader.GetStringProperty("discriminator");
            if (discriminator == null)
            {
                throw new JsonException();
            }

            return (FilterWhereClause?)(discriminator switch
            {
                nameof(UnaryFilterWhereClause) => JsonSerializer.Deserialize<UnaryFilterWhereClause>(ref reader, options),
                nameof(BinaryFilterWhereClause) => JsonSerializer.Deserialize<BinaryFilterWhereClause>(ref reader, options),
                _ => throw new NotSupportedException()
            }) ?? throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, FilterWhereClause value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
