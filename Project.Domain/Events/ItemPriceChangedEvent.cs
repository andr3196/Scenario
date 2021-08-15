using System;
using Scenario.Domain.SharedTypes;

namespace Project.Domain.Events
{
    public class ItemPriceChangedEvent : BaseEvent<Item>
    {
        public ItemPriceChangedEvent(Item item) : base(item)
        {
            Item = item;
        }

        public Item Item { get; }
    }
}
