using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Scenario.Domain.Modeling.Models.Constants;

namespace Scenario.Domain.Clauses
{
    public class ValueWhereClause
    {
        public string Type { get; set; }

        public string ValueType { get; set; }

        public string Value { get; set; }

    }
}
