using Microsoft.EntityFrameworkCore;

namespace Scenario.EntityFrameworkCore;

public class DatabaseContextConfiguration
{
    private Action<DbContextOptionsBuilder>? ConfigureContextOptionsBuilder { get; set; } 
    
    private Action<ModelBuilder>? ConfigureModelBuilder { get; set; } 
    
    public void ConfigureOptions(Action<DbContextOptionsBuilder> configureBuilder)
    {
        ConfigureContextOptionsBuilder = configureBuilder;
    }
    
    public void ConfigureModel(Action<ModelBuilder> configureBuilder)
    {
        ConfigureModelBuilder = configureBuilder;
    }
    
    public void Apply(DbContextOptionsBuilder builder)
    {
        if (ConfigureContextOptionsBuilder == null)
        {
            throw new InvalidOperationException(
                "The DatabaseContextConfiguration has not been configured yet. Call Configure.");
        }

        ConfigureContextOptionsBuilder.Invoke(builder);
    }
    
    public void Apply(ModelBuilder modelBuilder)
    {
        ConfigureModelBuilder?.Invoke(modelBuilder);
    }
    
}