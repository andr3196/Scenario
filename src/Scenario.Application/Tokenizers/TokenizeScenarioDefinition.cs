using MediatR;

namespace Scenario.Application.Tokenizers
{
    public class TokenizeScenarioDefinition : IRequest<TokenResult>
    {
        public string Definition { get; }

        public TokenizeScenarioDefinition(string definition)
        {
            Definition = definition;
        }
    }
}