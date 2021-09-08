using System;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using Scenario.Serialization;

namespace Scenario.Domain.Clauses
{
    public class RootNodeWhereClause
    {

        public IPredicateWhereClause Value { get; set; }

    }
}
