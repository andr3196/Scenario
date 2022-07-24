using AutoMapper;
using Scenario.Contracts.Commands;
using Scenario.Domain.Models;

namespace Scenario.Application.CommandHandlers.Update;

public class UpdateCommandMapper : Profile
{
    public UpdateCommandMapper()
    {
        CreateMap<UpdateScenarioCommand, ScenarioDefinition>()
            .ForMember(
                definition => definition.ConditionJson,
                opts => opts.MapFrom<PredicateClauseConditionValueResolver>())
            .ForMember(definition => definition.ConsequenceJson,
                opts => opts.MapFrom<ConsequenceClauseValueResolver>())
            .ForMember(c => c.Id, opts => opts.Ignore());
    }
}