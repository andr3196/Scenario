using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scenario.Domain.Persistence;

namespace Scenario.EntityFrameworkCore;

public class DatabaseScenarioEnityTypeConfiguration : IEntityTypeConfiguration<DatabaseScenario>
{
    public void Configure(EntityTypeBuilder<DatabaseScenario> builder)
    {
        builder.HasKey(s => s.Id);
    }
}