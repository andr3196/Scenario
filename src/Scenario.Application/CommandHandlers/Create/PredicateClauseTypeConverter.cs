using AutoMapper;
using Scenario.Domain.Models.Clauses;
using Scenario.Domain.Serialization.JsonConvertion;

namespace Scenario.Application.CommandHandlers.Create
{
    public class PredicateClauseTypeConverter : ITypeConverter<IPredicateClause, string>
    {
        private readonly IPredicateClauseConverter converter;

        public PredicateClauseTypeConverter(IPredicateClauseConverter converter)
        {
            this.converter = converter;
        }
        
        public string Convert(IPredicateClause source, string destination, ResolutionContext context)
        {
            return converter.Serialize(source);
        }
    }
}