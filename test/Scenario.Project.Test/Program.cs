using Scenario;
using Scenario.Domain.Serialization.JsonConvertion;
using Scenario.EntityFrameworkCore;
using Scenario.Project.Test;
using Scenario.Project.Test.Properties;

var appBuilder = WebApplication.CreateBuilder(args);

ConfigureServices(appBuilder.Services);

var app = appBuilder.Build();

ConfigureMiddleware(app, appBuilder.Environment);
ConfigureEndpoints(app, app.Services);

app.Run();

void ConfigureServices(IServiceCollection services)
{
    services.AddScenario(builder =>
    {
        builder.RegisterDomainAssembly<AssemblyReference>();
        builder.UseEntityFrameworkRepository<TestDatabaseConfigurationOptions>();
    });

    services.AddScenarioTest();

    services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new PredicateClauseConverter());
        });
}

void ConfigureMiddleware(IApplicationBuilder appBuilder, IWebHostEnvironment env)
{
    appBuilder.UseRouting();

    appBuilder.UseAuthorization();
}

void ConfigureEndpoints(IApplicationBuilder appBuilder, IServiceProvider services)
{
    appBuilder.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

    appBuilder.UseScenario();
}

namespace Scenario.Project.Test
{
    public partial class Program { }
}
