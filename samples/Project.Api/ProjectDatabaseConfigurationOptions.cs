using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Scenario.EntityFrameworkCore;
using Scenario.EntityFrameworkCore.EntityTypeConfigurations;

namespace Project.Api;

public class ProjectDatabaseConfigurationOptions : IConfigureOptions<DatabaseContextConfiguration>
{
    public void Configure(DatabaseContextConfiguration options)
    {
        options.ConfigureOptions(builder =>
        {
            builder.UseSqlite("Data Source=example-project2.db");
        });
        options.ConfigureModel(builder => builder.ApplyConfiguration(new ScenarioDefinitionConfiguration()));
    }
}