using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Scenario.Contracts.Commands;
using Scenario.Core;
using Scenario.Core.Exceptions;
using Scenario.Domain.Models;

namespace Scenario.Application.CommandHandlers.Update;

public class UpdateScenarioCommandHandler : IUpdateScenarioCommandHandler
{
    private readonly IRepository<ScenarioDefinition> scenarios;
    private readonly IMapper mapper;

    public UpdateScenarioCommandHandler(IRepository<ScenarioDefinition> scenarios, IMapper mapper)
    {
        this.scenarios = scenarios;
        this.mapper = mapper;
    }
    
    public async Task<Guid> Handle(UpdateScenarioCommand request, CancellationToken cancellationToken)
    {
        var scenario = await scenarios.SingleOrDefaultAsync(s => s.Id == request.Id, cancellationToken) ??
                       throw new ScenarioNotFoundException(); 
        mapper.Map(request, scenario);
        await scenarios.SaveAsync(cancellationToken);
        return scenario.Id;
    }
}