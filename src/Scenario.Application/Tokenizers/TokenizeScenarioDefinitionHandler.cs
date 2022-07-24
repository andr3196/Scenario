using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Scenario.Contracts.Exceptions.Tokenizers;

namespace Scenario.Application.Tokenizers
{
    public class TokenizeScenarioDefinitionHandler : IRequestHandler<TokenizeScenarioDefinition, TokenResult>
    {
        private string SpacesOutsideOfBracketsPattern = @"\s(?=(?:[^\}]*\{[^\}]*\})*[^\}]*$)"; 
        public Task<TokenResult> Handle(TokenizeScenarioDefinition request, CancellationToken cancellationToken)
        {
            var definition = request.Definition;
            
            if (string.IsNullOrWhiteSpace(definition))
            {
                throw new ArgumentNullException(nameof(definition));
            }

            if (definition[..4] != Keywords.When)
            {
                throw new ScenarioTokenizationMissingKeywordException(Keywords.When);
            }

            if (!definition.Contains(Keywords.Then))
            {
                throw new ScenarioTokenizationMissingKeywordException(Keywords.Then);
            }

            var components = definition[Keywords.When.Length..].Split(Keywords.Then);

            if (components.Length > 2)
            {
                throw new ScenarioTokenizationRepeatedKeywordException(Keywords.Then);
            }

            var (eventId, entityId, condition) = TokenizeEventAndFilter(components[0]); 
            
            return Task.FromResult(new TokenResult
            {
                EventEntity = entityId,
                EventId = eventId,
                WhereCondition = condition,
            });
        }

        private (string EventId, string? EntityId, Condition Filter) TokenizeEventAndFilter(string component)
        {
            var componentWords = Regex.Split(component, SpacesOutsideOfBracketsPattern)
                .Select(part => part.Trim())
                .Where(part => !string.IsNullOrWhiteSpace(part))
                .ToArray();

            return componentWords.Length switch
            {
                0 => throw new ScenarioTokenizationBadFormatException(),
                1 => (componentWords[0], null, new Condition()),
                2 when Keywords.StateEventTypes.Contains(componentWords[1]) => (componentWords[1], componentWords[0],
                    new Condition()),
                2 => throw new ScenarioTokenizationBadFormatException(),
                > 2 when componentWords[1] == Keywords.Where => (componentWords[0], null,
                    ExtractCondition(componentWords[2..])),
                > 3 when componentWords[2] == Keywords.Where => (componentWords[1], componentWords[0],
                    ExtractCondition(componentWords[3..])),
                _ => throw new ScenarioTokenizationBadFormatException()
            };
        }

        private Condition ExtractCondition(string[] componentWords)
        {
            throw new NotImplementedException();
        }
    }
}