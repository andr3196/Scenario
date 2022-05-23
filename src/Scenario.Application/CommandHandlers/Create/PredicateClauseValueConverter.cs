using AutoMapper;
using Scenario.Domain.Models.Clauses;
using Scenario.Domain.Serialization.JsonConvertion;

namespace Scenario.Application.CommandHandlers.Create
{
    public class PredicateClauseValueConverter : IValueConverter<IPredicateClause, string>
    {
        private readonly IPredicateClauseConverter converter;

        public PredicateClauseValueConverter(IPredicateClauseConverter converter)
        {
            this.converter = converter;
        }

        public string Convert(IPredicateClause sourceMember, ResolutionContext context)
        {
            return converter.Serialize(sourceMember);
        }
    }
}