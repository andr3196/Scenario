using System;
using Project.Domain.Events;
using Scenario.Domain.Modeling.Attributes;

namespace Project.Domain
{
    [ScenarioEnabled]
    public class Product: Entity
    {
        public int Quantity { get; set; }

        public long ItemId { get; set; }

        public Item Item { get; set; }

        [ScenarioEvent(typeof(CreatedEvent<Product>))]
        public override void Created()
        {
            Raise(new CreatedEvent<Product>(this));
        }

        [ScenarioEvent(typeof(CreatedEvent<Product>))]
        public override void Updated()
        {
            Raise(new UpdatedEvent<Product>(this));
        }
    }
}
