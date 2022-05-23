using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Scenario.Domain.Serialization.JsonConvertion;

namespace Scenario.Api.Test.Extensions
{
    public static class HttpClientExtensions
    {
        private static readonly JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new PredicateClauseConverter(),
            },
        };
        
        public static async Task<TResponse?> GetAsync<TResponse>(this HttpClient client, string uri, CancellationToken cancellationToken = default)
        {
            var response = await client.GetAsync(uri, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
        }
        
        public static async Task<TResponse?> PostAsync<TPayload, TResponse>(this HttpClient client, string uri, TPayload payload, CancellationToken cancellationToken = default)
        {
            var stringContent = JsonSerializer.Serialize(payload, options);
            var httpContent = new StringContent(stringContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(uri, httpContent, cancellationToken);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
        }
    }
}