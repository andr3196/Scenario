using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project.Api.Persistence;
using Project.Domain;

namespace Project.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly DatabaseContext databaseContext;

        public ProjectController(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        [HttpPost]
        [Route("raise/{type}")]
        public IActionResult Raise(string type, CancellationToken cancellationToken)
        {
            if (type == "AccountCreated")
            {
                var customer = databaseContext.Set<Customer>().SingleOrDefault();
                if (customer == null)
                {
                    customer = new Customer
                    {
                        Name = "Customer 1",
                        Password = "MySecret"
                    };
                    databaseContext.Set<Customer>().Add(customer);
                    databaseContext.SaveChanges();
                }

                customer.AccountCreated();
            }
            if( type == "ItemCreated")
            {
                var items = databaseContext.Set<Item>().ToList();
                var item = new Item
                {
                    Price = 23,
                    Stock = 50 -  items.Count,
                    Title = "Nice item",
                };
                databaseContext.Set<Item>().Add(item);
            }
            
            databaseContext.SaveChanges();

            return Ok();
        }
    }
}
