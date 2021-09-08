using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using Scenario.Domain.Modeling.Models.Filters;
using Scenario.Serialization;

namespace Scenario.Domain.Clauses
{
    public abstract class FilterWhereClause : IPredicateWhereClause
    {
        public string Operator { get; set; }

        public abstract string Discriminator { get; }
    }
}
