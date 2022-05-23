using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Scenario.Api.Controllers;

namespace Scenario.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddScenarioApi(this IServiceCollection services)
        {
            return services
                .AddMediatR(typeof(DependencyInjection))
                .AddTransient<ScenarioController>()
                .AddTransient<ScenarioModelController>();
        }

        public static IApplicationBuilder UseScenario(this IApplicationBuilder app)
        {
            // app.UseRouting();
            //
            // app.UseCors(builder =>
            //     builder.WithOrigins("http://localhost:4200")
            //         .AllowAnyHeader()
            //         .AllowAnyMethod()
            //         .AllowCredentials());
            //
            // app.UseAuthorization();
            //
            // app.UseEndpoints(endpoints =>
            // {
            //     endpoints.MapControllers();
            // });

            return app;
        }
    }
}
