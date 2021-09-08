using System;
using System.Linq.Expressions;

namespace Scenario.Domain.Clauses
{
    public interface IPredicateWhereClause
    {
        public string Discriminator { get; }
    }
}
