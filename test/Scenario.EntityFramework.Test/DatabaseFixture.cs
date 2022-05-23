using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Scenario.EntityFrameworkCore;
using Scenario.Project.Test;

namespace Scenario.EntityFramework.Test;

public class DatabaseFixture<TContext>: IDisposable
    where TContext : DbContext
{
    private readonly DatabaseContext context;

    public DatabaseFixture()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();
        

        var configuration = new DatabaseContextConfiguration();
        var configureOptions = new TestDatabaseConfigurationOptions();
        configureOptions.Configure(configuration);
        context = new DatabaseContext(optionsBuilder.Options, new TestSnapshot<DatabaseContextConfiguration>(configuration));
        context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        context.Database.EnsureDeleted();
    }
}