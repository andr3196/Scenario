using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Scenario.EntityFrameworkCore;
using Scenario.EntityFrameworkCore.EntityTypeConfigurations;

namespace Scenario.Project.Test;

public class TestDatabaseConfigurationOptions : IConfigureOptions<DatabaseContextConfiguration>
{
    public void Configure(DatabaseContextConfiguration options)
    {
        options.ConfigureOptions(builder =>
        {
            builder.UseSqlite("Data Source=scenario-test.db");
        });
        options.ConfigureModel(builder =>
        {
            builder.ApplyConfiguration(new ScenarioDefinitionConfiguration());
            builder.ApplyConfiguration(new DatabaseScenarioEnityTypeConfiguration());
        });
    }
}