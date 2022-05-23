using MediatR;

namespace Scenario.Contracts.Queries;

public record GetScenarioByIdQuery : IRequest<ScenarioDetails>
{
    public Guid Id { get; set; }
}