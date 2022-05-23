using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scenario.Contracts;
using Scenario.Contracts.Queries;
using Scenario.Core;
using Scenario.Core.Exceptions;
using Scenario.Domain.Models;

namespace Scenario.Application.QueryHandlers.Details;

public class GetScenarioByIdQueryHandler : IGetScenarioByIdQueryHandler
{
    private readonly ISource<ScenarioDefinition> scenarios;
    private readonly IMapper mapper;

    public GetScenarioByIdQueryHandler(ISource<ScenarioDefinition> scenarios, IMapper mapper)
    {
        this.scenarios = scenarios;
        this.mapper = mapper;
    }
    
    public async Task<ScenarioDetails> Handle(GetScenarioByIdQuery request, CancellationToken cancellationToken)
    {
        var scenario = scenarios.Where(s => s.Id == request.Id);

        return await mapper.ProjectTo<ScenarioDetails>(scenario).SingleOrDefaultAsync(cancellationToken) ??
               throw new ScenarioNotFoundException();
    }
}