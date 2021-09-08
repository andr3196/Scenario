using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Scenario.Domain.Modeling.Models.Filters;

namespace Scenario.Domain.Clauses
{
    public class BinaryFilterWhereClause : FilterWhereClause
    {
        public ValueWhereClause Left { get; set; }

        public ValueWhereClause Right { get; set; }

        public override string Discriminator => nameof(BinaryFilterWhereClause);
    }
}
