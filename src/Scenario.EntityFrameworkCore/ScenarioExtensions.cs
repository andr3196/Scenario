using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Scenario.Configuration;
using Scenario.Core;
using Scenario.Domain.Persistence;

namespace Scenario.EntityFrameworkCore;

public static class ScenarioExtensions
{
    public static void UseEntityFrameworkRepository<TDatabaseConfigurationOptions>(this IScenarioConfigurationBuilder builder, Action<DbContextOptionsBuilder>? optionsAction = null)
    where TDatabaseConfigurationOptions : class, IConfigureOptions<DatabaseContextConfiguration>
    {
        builder.UsePersistenceRepository<ScenarioRepository>(services =>
        {
            services.AddScoped<IConfigureOptions<DatabaseContextConfiguration>, TDatabaseConfigurationOptions>()
                .AddTransient(typeof(IRepository<>), typeof(Repository<>))
                .AddTransient(typeof(ISource<>), typeof(Source<>))
                .AddPersistenceModel()
                .AddDbContext<DatabaseContext>(optionsAction)
                .PostConfigure<DatabaseContext>(context => context.Database.EnsureCreated());
            services.AddScoped<ScenarioConfigurationAction>(_ => ConfigurationActions.EnsureDatabaseCreated);
        });
    }
}