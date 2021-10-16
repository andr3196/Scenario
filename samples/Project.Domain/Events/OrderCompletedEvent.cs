using System;
using Scenario.Domain.SharedTypes;

namespace Project.Domain.Events
{
    public class OrderCompletedEvent : BaseEvent<Order>
    {
        public OrderCompletedEvent(Order order) : base(order)
        {
            Order = order;
        }

        public Order Order { get; }
    }
}
