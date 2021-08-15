using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Scenario.Domain.SharedTypes;

namespace Scenario.Domain.ScenarioDefinitions
{
    public class ScenarioDefinition
    {


        public ScenarioDefinition()
        {
            
        }

        public string Key { get; set; }

        public Type EventType { get; set; }

        public Func<object,bool> Condition { get; set; }

        public IEnumerable<string> IncludedProperties { get; set; }

        public ScenarioEventHandlerAsync Handler { get; set; }
    }
}
