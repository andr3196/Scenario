using System;
using Scenario.Domain.SharedTypes;

namespace Project.Domain.Events
{
    public class PasswordChangedEvent: BaseEvent<Customer>
    {
        public PasswordChangedEvent(Customer customer) : base(customer)
        {
            Customer = customer;
        }

        public Customer Customer { get; }
    }
}
