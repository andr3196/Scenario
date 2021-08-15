using System;
using System.Collections.Generic;

namespace Scenario.Domain.Modeling.Models
{
    public class ScenarioSetup
    {
        public ScenarioSetup()
        {
            
        }

        public IEnumerable<Entity> Entities { get; set; }

        public IDictionary<string, IEnumerable<Event>> EventsDictionary { get; set; }

        public IEnumerable<Constant> Constants { get; set; }

        public IDictionary<string, IEnumerable<Filter>> FilterDictionary { get; set; }

        public IEnumerable<Logical> Logicals { get; set; }

        public IEnumerable<Consequence> Consequences { get; set; }
    }
}
