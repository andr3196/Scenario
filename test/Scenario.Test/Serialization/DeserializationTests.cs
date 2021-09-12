using System.Buffers;
using System.Text;
using System.Text.Json;
using Scenario.Domain.Clauses;
using Scenario.Domain.JsonConvertion;
using Xunit;

namespace Scenario.Test.Serialization
{
    public class DeserializationTests
    {
        [Fact]
        public void ShouldDeSerializeRootClauseWhenUnaryPredicateClause()
        {
            var serializer = new RootClauseConverter();
            var buffer = new ArrayBufferWriter<byte>(100);
            var jsonInput = "{\"Value\":{\"Discriminator\":\"UnaryPredicateClause\",\"Value\":null}}";
            var byteInput = Encoding.UTF8.GetBytes(jsonInput);
            var reader = new Utf8JsonReader(byteInput);

            var rootClause = serializer.Read(ref reader, typeof(RootClause), new JsonSerializerOptions());
            
            Assert.NotNull(rootClause);
            Assert.NotNull(rootClause.Value);
            Assert.Equal(typeof(UnaryPredicateClause), rootClause.Value.GetType());
        }
    }
}
