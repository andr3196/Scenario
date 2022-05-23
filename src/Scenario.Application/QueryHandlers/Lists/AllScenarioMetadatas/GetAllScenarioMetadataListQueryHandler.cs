using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Scenario.Contracts;
using Scenario.Core;
using Scenario.Domain.Models;

namespace Scenario.Application.QueryHandlers.Lists.AllScenarioMetadatas;

public class GetAllScenarioMetadataListQueryHandler : IGetAllScenarioMetadataListQueryHandler, IRequest<ScenarioMetadataDto[]>
{
    private readonly ISource<ScenarioDefinition> scenarios;
    private readonly IMapper mapper;

    public GetAllScenarioMetadataListQueryHandler(ISource<Scenario.Domain.Models.ScenarioDefinition> scenarios, IMapper mapper)
    {
        this.scenarios = scenarios;
        this.mapper = mapper;
    }
    
    public Task<ScenarioMetadataDto[]> Handle(GetAllScenarioMetadataListQueryHandler request, CancellationToken cancellationToken)
    {
        return mapper.ProjectTo<ScenarioMetadataDto>(scenarios).ToArrayAsync(cancellationToken);
    }
}