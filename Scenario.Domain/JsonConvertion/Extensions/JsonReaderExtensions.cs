using System;
using System.Text.Json;

namespace Scenario.Domain.JsonConvertion.Extensions
{
    public static class JsonReaderExtensions
    {
        public static string? GetStringProperty(this Utf8JsonReader reader, string propertyName)
        {
            Utf8JsonReader readerClone = reader;
            if (readerClone.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            while (readerClone.Read())
            {
                if(readerClone.TokenType == JsonTokenType.EndObject)
                {
                    return null;
                }

                if(readerClone.TokenType == JsonTokenType.PropertyName && readerClone.GetString() == propertyName)
                {
                    readerClone.Read();
                    return readerClone.GetString();
                }
                readerClone.Read();
            }
            return null;
        }

        public static Utf8JsonReader FastForwardToValueOfSymbol(this Utf8JsonReader reader, string symbol)
        {

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    throw new JsonException();
                }

                if (reader.TokenType == JsonTokenType.PropertyName && reader.GetString() == symbol)
                {
                    reader.Read();
                    return reader;
                }
                reader.Read();
            }
            throw new JsonException();
        }
    }
}
