using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Scenario.Domain.Models;

namespace Scenario.EntityFrameworkCore.EntityTypeConfigurations;

public class ScenarioDefinitionConfiguration : IEntityTypeConfiguration<ScenarioDefinition>
{
    public void Configure(EntityTypeBuilder<ScenarioDefinition> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Title).HasMaxLength(250);
    }
}