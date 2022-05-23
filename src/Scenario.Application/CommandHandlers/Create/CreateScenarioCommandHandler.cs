using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Scenario.Contracts.Commands;
using Scenario.Core;
using Scenario.Domain.Models;

namespace Scenario.Application.CommandHandlers.Create;

public class CreateScenarioCommandHandler : ICreateScenarioCommandHandler
{
    private readonly IRepository<ScenarioDefinition> scenarios;
    private readonly IMapper mapper;

    public CreateScenarioCommandHandler(IRepository<ScenarioDefinition> scenarios, IMapper mapper)
    {
        this.scenarios = scenarios;
        this.mapper = mapper;
    }
    
    public async Task<Guid> Handle(CreateScenarioCommand request, CancellationToken cancellationToken)
    {
        var scenario = mapper.Map<ScenarioDefinition>(request);
        await scenarios.AddAsync(scenario, cancellationToken);
        return scenario.Id;
    }
}