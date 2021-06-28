using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Scenario.Infrastructure
{
    public interface IDatabaseContext
    {

        public DbSet<T> Set<T>() where T : class;

        public Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
