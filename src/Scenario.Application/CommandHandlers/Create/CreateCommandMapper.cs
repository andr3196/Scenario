using AutoMapper;
using Scenario.Contracts.Commands;
using Scenario.Domain.Models;
using Scenario.Domain.Serialization.JsonConvertion;

namespace Scenario.Application.CommandHandlers.Create;

public class CreateCommandMapper : Profile
{
    public CreateCommandMapper()
    {
        CreateMap<CreateScenarioCommand, ScenarioDefinition>()
            .ForMember(
                definition => definition.ConditionJson,
                opts => opts.MapFrom<PredicateClauseConditionValueResolver>())
            .ForMember(definition => definition.ConsequenceJson,
                opts => opts.MapFrom<ConsequenceClauseValueResolver>());
    }
}