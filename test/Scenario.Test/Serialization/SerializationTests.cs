using System;
using System.Buffers;
using System.IO;
using System.Text;
using System.Text.Json;
using Scenario.Domain.Clauses;
using Scenario.Domain.JsonConvertion;
using Xunit;

namespace Scenario.Test.Serialization
{
    public class SerializationTests
    {
        [Fact]
        public void ShouldSerializeNullPredicateClause()
        {
            var serializer = new PredicateClauseConverter();
            var buffer = new ArrayBufferWriter<byte>(100);
            var writer = new Utf8JsonWriter(buffer);
            IPredicateClause predicate = null;

            serializer.Write(writer, predicate, new JsonSerializerOptions());
            writer.Flush();

            var expectedOutPut = "null";
            var result = Encoding.Default.GetString(buffer.WrittenSpan);
            Assert.Equal(expectedOutPut, result);
        }

        [Fact]
        public void ShouldSerializeUnaryPredicateClause()
        {
            var serializer = new PredicateClauseConverter();
            var buffer = new ArrayBufferWriter<byte>(100);
            var writer = new Utf8JsonWriter(buffer);
            IPredicateClause predicate = new UnaryPredicateClause(null);

            serializer.Write(writer, predicate, new JsonSerializerOptions());
            writer.Flush();

            var expectedOutPut = "{\"discriminator\":\"UnaryPredicateClause\",\"value\":null}";
            var result = Encoding.Default.GetString(buffer.WrittenSpan);
            Assert.Equal(expectedOutPut, result);
        }

        [Fact]
        public void ShouldSerializeBinaryPredicateClause()
        {
            var serializer = new PredicateClauseConverter();
            var buffer = new ArrayBufferWriter<byte>(100);
            var writer = new Utf8JsonWriter(buffer);
            IPredicateClause predicate = new BinaryPredicateClause(null, null, "combinator");

            serializer.Write(writer, predicate, new JsonSerializerOptions());
            writer.Flush();

            var expectedOutPut = "{\"discriminator\":\"BinaryPredicateClause\",\"combinator\":\"combinator\",\"left\":null,\"right\":null}";
            var result = Encoding.Default.GetString(buffer.WrittenSpan);
            Assert.Equal(expectedOutPut, result);
        }
    }
}
