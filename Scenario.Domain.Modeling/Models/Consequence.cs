using System;
using System.Collections.Generic;

namespace Scenario.Domain.Modeling.Models
{
    public class Consequence : Suggestable
    {
        public Consequence()
        {
        }

        public string ParameterType { get; set; }

        public string HandlerType { get; set; }

        public IEnumerable<Parameter> Parameters { get; set; }
    }
}
