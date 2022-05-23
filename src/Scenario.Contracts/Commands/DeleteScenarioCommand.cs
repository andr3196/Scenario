using MediatR;
using Scenario.Domain.Models.Clauses;

namespace Scenario.Contracts.Commands;

public record DeleteScenarioCommand : IRequest
{
    public Guid Id { get; set; }
}