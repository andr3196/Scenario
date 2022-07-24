using Microsoft.Extensions.Options;
using Scenario.EntityFrameworkCore.EntityTypeConfigurations;

namespace Scenario.EntityFrameworkCore
{
    public class ScenarioDatabaseConfigurationOptions : IConfigureOptions<DatabaseContextConfiguration>
    {
        public void Configure(DatabaseContextConfiguration options)
        {
            options.ConfigureModel(builder => builder.ApplyConfiguration(new DatabaseScenarioEnityTypeConfiguration()));
        }
    }
}