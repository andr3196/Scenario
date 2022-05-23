using System.Buffers;
using System.Text;
using System.Text.Json;
using Scenario.Domain.Models.Clauses;
using Scenario.Domain.Serialization.JsonConvertion;
using Xunit;

namespace Scenario.Test.Serialization
{
    public class DeserializationTests
    {
        [Fact]
        public void ShouldDeserializeRootClauseWhenUnaryPredicateClause()
        {
            var serializer = new PredicateClauseConverter();
            var buffer = new ArrayBufferWriter<byte>(100);
            var jsonInput = "{\"Discriminator\":\"UnaryPredicateClause\",\"Value\":null}";
            var byteInput = Encoding.UTF8.GetBytes(jsonInput);
            var reader = new Utf8JsonReader(byteInput);

            var clause = serializer.Read(ref reader, typeof(IPredicateClause), new JsonSerializerOptions());
            
            Assert.NotNull(clause);
            Assert.Equal(typeof(UnaryPredicateClause), clause.GetType());
        }

        [Fact]
        public void ShouldDeserializeRootClauseWhenBinaryPredicateClause()
        {
            var serializer = new PredicateClauseConverter();
            var buffer = new ArrayBufferWriter<byte>(100);
            var jsonInput = "{\"Discriminator\":\"BinaryPredicateClause\",\"Left\":null,\"Right\":null}";
            var byteInput = Encoding.UTF8.GetBytes(jsonInput);
            var reader = new Utf8JsonReader(byteInput);

            var clause = serializer.Read(ref reader, typeof(IPredicateClause), new JsonSerializerOptions());
            
            Assert.NotNull(clause);
            Assert.Equal(typeof(BinaryPredicateClause), clause.GetType());
        }

        [Fact]
        public void ShouldDeserializeRootClauseWhenUnaryPredicateWithNestedUnaryOperator()
        {
            var serializer = new PredicateClauseConverter();
            var options = new JsonSerializerOptions();
            options.Converters.Add(serializer);
            var jsonInput = @"{
                ""Discriminator"":""UnaryPredicateClause"",
                ""Value"": {
                    ""Discriminator"": ""UnaryOperatorClause"",
                    ""OperatorKey"":""isNotNull"",
                    ""Value"":null
                    }
                }";
            var byteInput = Encoding.UTF8.GetBytes(jsonInput);
            var reader = new Utf8JsonReader(byteInput);

            var clause = serializer.Read(ref reader, typeof(IPredicateClause), options);
            
            Assert.NotNull(clause);
            var unaryClause = clause as UnaryPredicateClause;
            Assert.NotNull(unaryClause);
            var operatorClause = unaryClause.Value as UnaryOperatorClause;
            Assert.NotNull(operatorClause);
            Assert.Equal("isNotNull", operatorClause.OperatorKey);
        }

        [Fact]
        public void ShouldDeserializeRootClauseWhenBinaryPredicateWithNestedBinaryOperator()
        {
            var serializer = new PredicateClauseConverter();
            var options = new JsonSerializerOptions();
            options.Converters.Add(serializer);
            var jsonInput = @"{
                ""Discriminator"":""BinaryPredicateClause"",
                ""Combinator"":""LOGICAL_AND"",
                ""Left"": {
                    ""Discriminator"": ""UnaryOperatorClause"",
                    ""OperatorKey"":""isNotNull"",
                    ""Value"":null
                    },
                ""Right"": {
                    ""Discriminator"": ""BinaryOperatorClause"",
                    ""OperatorKey"":""compare"",
                    ""Left"":null,
                    ""Right"":null
                    }
                }";
            var byteInput = Encoding.UTF8.GetBytes(jsonInput);
            var reader = new Utf8JsonReader(byteInput);

            var clause = serializer.Read(ref reader, typeof(IPredicateClause), options);
            
            Assert.NotNull(clause);
            var binaryClause = clause as BinaryPredicateClause;
            Assert.NotNull(binaryClause);
            Assert.Equal("LOGICAL_AND", binaryClause.Combinator);
            var leftClause = binaryClause.Left as UnaryOperatorClause;
            Assert.NotNull(leftClause);

            var rightClause = binaryClause.Right as BinaryOperatorClause;
            Assert.NotNull(rightClause);
        }
    }
}
