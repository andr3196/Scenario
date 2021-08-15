using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scenario.Domain.Clauses;

namespace Scenario.Serialization
{
    public class PredicateWhereClauseConverter : JsonConverter<IPredicateWhereClause>
    {
        private readonly PredicateWhereClauseResolver predicateWhereClauseResolver;

        public PredicateWhereClauseConverter(PredicateWhereClauseResolver predicateWhereClauseResolver)
        {
            this.predicateWhereClauseResolver = predicateWhereClauseResolver;
        }

        public override IPredicateWhereClause Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
            if (propertyName != "Discriminator")
            {
                throw new JsonException();
            }

            readerClone.Read();
            if (readerClone.TokenType != JsonTokenType.String)
            {
                throw new JsonException();
            }

            var predicateClauseType = predicateWhereClauseResolver(readerClone.GetString());

            if(predicateClauseType == null)
            {
                throw new JsonException();
            }

            return (IPredicateWhereClause)JsonSerializer.Deserialize(ref reader, predicateClauseType);
        }

        public override void Write(Utf8JsonWriter writer, IPredicateWhereClause value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
