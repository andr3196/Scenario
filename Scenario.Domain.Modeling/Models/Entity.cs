using System;
using System.Collections.Generic;

namespace Scenario.Domain.Modeling.Models
{
    public class Entity : Suggestable
    {
        public Entity()
        {
        }

        public IEnumerable<Property> Properties { get; set; }
    }
}
