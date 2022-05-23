using MediatR;
using Scenario.Contracts;

namespace Scenario.Application.QueryHandlers.Lists.AllScenarioMetadatas;

public interface IGetAllScenarioMetadataListQueryHandler : IRequestHandler<GetAllScenarioMetadataListQueryHandler, ScenarioMetadataDto[]>
{
    
}