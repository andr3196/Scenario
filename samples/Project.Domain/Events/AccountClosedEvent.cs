
namespace Project.Domain.Events
{
    public class AccountClosedEvent : BaseEvent<Customer>
    {
        public AccountClosedEvent(Customer customer) : base(customer)
        {
            Customer = customer;
        }

        public Customer Customer { get; }
    }
}
