using MediatR;
using Scenario.Contracts;
using Scenario.Contracts.Queries;

namespace Scenario.Application.QueryHandlers.Lists.AllScenarios;

public interface IGetAllScenariosQueryHandler : IRequestHandler<GetAllScenariosQuery, ScenarioDefinitionDto[]>
{
    
}