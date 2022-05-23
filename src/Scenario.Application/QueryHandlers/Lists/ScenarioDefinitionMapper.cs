using AutoMapper;
using Scenario.Contracts;
using Scenario.Contracts.Queries;
using Scenario.Domain.Models;
using Scenario.Domain.Models.Clauses;

namespace Scenario.Application.QueryHandlers.Lists;

public class ScenarioDefinitionMapper : Profile
{
    public ScenarioDefinitionMapper()
    {
        CreateMap<ScenarioDefinition, ScenarioDefinitionDto>()
            .ReverseMap();
        
        CreateMap<ScenarioDefinition, ScenarioMetadataDto>()
            .ReverseMap();

        CreateMap<Condition, IPredicateClause>()
            .ReverseMap();

        CreateMap<Consequence, ConsequenceClause>()
            .ReverseMap();
    }
}