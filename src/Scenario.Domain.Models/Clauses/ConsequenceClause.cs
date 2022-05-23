using System.Collections.Generic;

namespace Scenario.Domain.Models.Clauses
{
    public class ConsequenceClause
    {
        public ConsequenceClause(string key, Dictionary<string, ValueClause> parameters)
        {
            Key = key;
            Parameters = parameters;
        }

        public string Key { get; }
        public Dictionary<string, ValueClause> Parameters { get; }
    }
}
