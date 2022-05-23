using MediatR;
using Scenario.Contracts;
using Scenario.Contracts.Queries;

namespace Scenario.Application.QueryHandlers.Details;

public interface IGetScenarioByIdQueryHandler : IRequestHandler<GetScenarioByIdQuery, ScenarioDetails>
{
    
}