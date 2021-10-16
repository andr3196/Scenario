using System;
using Scenario.Domain.SharedTypes;

namespace Project.Domain.Events
{
    public class OrderStartedEvent : BaseEvent<Order>
    {
        public OrderStartedEvent(Order order) : base(order)
        {
            Order = order;
        }

        public Order Order { get; }
    }
}
