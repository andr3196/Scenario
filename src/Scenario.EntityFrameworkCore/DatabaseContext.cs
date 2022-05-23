using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Scenario.EntityFrameworkCore;

public class DatabaseContext : DbContext
{
    private readonly DatabaseContextConfiguration configuration;
    public DatabaseContext(
        DbContextOptions<DatabaseContext> options,
        IOptionsSnapshot<DatabaseContextConfiguration> configurationOptions) : base(options)
    {
        configuration = configurationOptions.Value;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        configuration.Apply(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        configuration.Apply(optionsBuilder);
        base.OnConfiguring(optionsBuilder);
    }
}