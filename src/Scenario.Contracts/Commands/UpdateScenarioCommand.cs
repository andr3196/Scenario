using MediatR;
using Scenario.Domain.Models.Clauses;

namespace Scenario.Contracts.Commands;

public record UpdateScenarioCommand : IRequest<Guid>
{
    public Guid Id { get; init; }
    
    public string? Title { get; init; }

    public IPredicateClause Condition { get; set; }
    
    public ConsequenceClause Consequence { get; set; }
    
    public bool IsActive { get; set; }
}