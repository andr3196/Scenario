using MediatR;

namespace Scenario.Contracts.Commands
{
    public class CreateScenarioDefinition : IRequest<Guid>
    {
        
        public CreateScenarioDefinition(string definition)
        {
            Definition = definition;
        }
        
        public string Definition { get; }
    }
}