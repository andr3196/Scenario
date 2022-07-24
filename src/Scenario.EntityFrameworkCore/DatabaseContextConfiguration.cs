using Microsoft.EntityFrameworkCore;

namespace Scenario.EntityFrameworkCore;

public class DatabaseContextConfiguration
{
    private List<Action<DbContextOptionsBuilder>> ConfigureContextOptionsBuilders { get; set; } =
        new List<Action<DbContextOptionsBuilder>>();

    private List<Action<ModelBuilder>> ConfigureModelBuilders { get; } = new();
    
    public void ConfigureOptions(Action<DbContextOptionsBuilder> configureBuilder)
    {
        ConfigureContextOptionsBuilders.Add(configureBuilder);
    }
    
    public void ConfigureModel(Action<ModelBuilder> configureBuilder)
    {
        ConfigureModelBuilders.Add(configureBuilder);
    }
    
    public void Apply(DbContextOptionsBuilder builder)
    {
        if (!ConfigureContextOptionsBuilders.Any())
        {
            throw new InvalidOperationException(
                "The DatabaseContextConfiguration has not been configured yet. Call Configure.");
        }

        ConfigureContextOptionsBuilders.ForEach(configureBuilder => configureBuilder(builder));
    }
    
    public void Apply(ModelBuilder modelBuilder)
    {
        ConfigureModelBuilders.ForEach(configureAction => configureAction(modelBuilder));
    }
    
}