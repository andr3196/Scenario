using System;
using Scenario.Domain.Modeling.Attributes;

namespace Project.Domain
{
    [ScenarioEnabled]
    public class Product: Entity
    {
        public int Quantity { get; set; }

        public long ItemId { get; set; }

        public Item Item { get; set; }
    }
}
