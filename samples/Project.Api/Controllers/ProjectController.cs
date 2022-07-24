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
        private readonly ProjectDatabaseContext projectDatabaseContext;

        public ProjectController(ProjectDatabaseContext projectDatabaseContext)
        {
            this.projectDatabaseContext = projectDatabaseContext;
        }

        [HttpPost]
        [Route("raise/{type}")]
        public async Task<IActionResult> Raise(string type, CancellationToken cancellationToken)
        {
            if (type == "AccountCreated")
            {
                var customer = projectDatabaseContext.Set<Customer>().SingleOrDefault();
                if (customer == null)
                {
                    customer = new Customer
                    {
                        Name = "Customer 1",
                        Password = "MySecret"
                    };
                    projectDatabaseContext.Set<Customer>().Add(customer);
                    await projectDatabaseContext.SaveChangesAsync(cancellationToken);
                }

                customer.AccountCreated();
            }
            if( type == "ItemCreated")
            {
                var items = projectDatabaseContext.Set<Item>().ToList();
                var item = new Item
                {
                    Price = 23,
                    Stock = 50 -  items.Count,
                    Title = "Nice item",
                };
                projectDatabaseContext.Set<Item>().Add(item);
            }
            
            await projectDatabaseContext.SaveChangesAsync(cancellationToken);

            return Ok();
        }
    }
}
