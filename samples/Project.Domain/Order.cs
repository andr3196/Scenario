using System;
using System.Collections.Generic;
using Project.Domain.Events;
using Scenario.Domain.Modeling.Attributes;

namespace Project.Domain
{
    [ScenarioEnabled]
    public class Order: Entity
    {
        public string Status { get; set; }

        public Customer Customer { get; set; }

        public DateTime OrderDate { get; set; }

        public IEnumerable<Product> Products { get; set; }

        [ScenarioEvent(typeof(OrderStartedEvent))]
        public void Started()
        {
            Raise(new OrderStartedEvent(this));
        }

        [ScenarioEvent(typeof(OrderCompletedEvent))]
        public void Completed()
        {
            Raise(new OrderCompletedEvent(this));
        }

        [ScenarioEvent(typeof(CreatedEvent<Order>))]
        public override void Created()
        {
            Raise(new CreatedEvent<Order>(this));
        }

        [ScenarioEvent(typeof(CreatedEvent<Order>))]
        public override void Updated()
        {
            Raise(new UpdatedEvent<Order>(this));
        }
    }
}
