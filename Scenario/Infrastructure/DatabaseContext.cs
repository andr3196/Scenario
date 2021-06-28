using System;
using Microsoft.EntityFrameworkCore;

namespace Scenario.Infrastructure
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(@"Data Source=/tmp/blogging.db");
    }
}
