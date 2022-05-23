using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scenario.Contracts;
using Scenario.Contracts.Queries;
using Scenario.Core;
using Scenario.Domain.Models;

namespace Scenario.Application.QueryHandlers.Lists.AllScenarios;

public class GetAllScenariosQueryHandler : IGetAllScenariosQueryHandler
{
    private readonly ISource<ScenarioDefinition> scenarios;
    private readonly IMapper mapper;

    public GetAllScenariosQueryHandler(ISource<ScenarioDefinition> scenarios, IMapper mapper)
    {
        this.scenarios = scenarios;
        this.mapper = mapper;
    }
    
    public async Task<ScenarioDefinitionDto[]> Handle(GetAllScenariosQuery request, CancellationToken cancellationToken)
    {
        return await mapper.ProjectTo<ScenarioDefinitionDto>(scenarios).ToArrayAsync(cancellationToken);
    }
}