using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Project.Api;
using Project.Api.Persistence;
using Project.Api.Services;
using Project.Domain.EventHandlers;
using Project.Domain.Handlers;
using Scenario;

var builder = WebApplication.CreateBuilder(args);

ConfigureConfiguration(builder.Configuration);
ConfigureServices(builder.Services);

var app = builder.Build();

ConfigureMiddleware(app, builder.Environment);
ConfigureEndpoints(app, app.Services);

app.Run();

void ConfigureConfiguration(ConfigurationManager configuration)
{
    
}

void ConfigureServices(IServiceCollection services)
{
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Scenario", Version = "v1" });
    });

    services.AddScenarioProject();
    services.AddScoped(typeof(IEventHandler<>), typeof(ScenarioEventHandler<>));

    services.AddControllers()
        .AddJsonOptions(options =>
        {
            //options.JsonSerializerOptions.Converters.Add(new PredicateWhereClauseConverter());
        });
    services.AddDbContext<ProjectDatabaseContext>();
    services.AddTransient<SendEmailHandler>();
}

void ConfigureMiddleware(IApplicationBuilder appBuilder, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        appBuilder.UseDeveloperExceptionPage();
        appBuilder.UseSwagger();
        appBuilder.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Scenario v1"));
    }

    using var scope = appBuilder.ApplicationServices.CreateScope();
    scope.ServiceProvider.GetRequiredService<ProjectDatabaseContext>().Database.EnsureCreated();

    appBuilder.UseHttpsRedirection();

    appBuilder.UseRouting();

    appBuilder.UseCors(corsBuilder =>
        corsBuilder.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());

    appBuilder.UseAuthorization();
}

void ConfigureEndpoints(IApplicationBuilder appBuilder, IServiceProvider services)
{
    appBuilder.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });

    appBuilder.UseCurrentProject();
    appBuilder.UseScenario();
}

namespace Project.Api
{
    public partial class Program { }
}
