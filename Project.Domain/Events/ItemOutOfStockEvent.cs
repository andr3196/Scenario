using System;
using Scenario.Domain.SharedTypes;

namespace Project.Domain.Events
{
    public class ItemOutOfStockEvent : BaseEvent<Item>
    {
        public ItemOutOfStockEvent(Item item) : base(item) 
        {
            Item = item;
        }

        public Item Item { get; }
    }
}
