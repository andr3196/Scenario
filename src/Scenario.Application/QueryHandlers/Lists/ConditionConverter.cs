using AutoMapper;
using Scenario.Application.Services.ScenarioParsing;
using Scenario.Domain.Models.Clauses;

namespace Scenario.Application.QueryHandlers.Lists;

public class ConditionConverter : IValueConverter<string, IPredicateClause>
{

    public ConditionConverter()
    {
    }
    
    public IPredicateClause Convert(string conditionJson, ResolutionContext context)
    {
        return new UnaryOperatorClause(new ValueClause("string", "string", "hello"), "not null");
    }
}