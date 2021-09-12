﻿using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Scenario.Domain;
using Scenario.Domain.Modeling;
using Scenario.Domain.SharedTypes;
using Scenario.Infrastructure;
using Scenario.Serialization;
using Scenario.Services;
using Scenario.Services.ExpressionBuilding;

namespace Scenario
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddScenario(this IServiceCollection services, Assembly domainAssembly)
        {
            services.AddControllers();
            services
                .AddScenarioModelling(domainAssembly)
                .AddTransient<IScenarioModelService, ScenarioModelService>()
                .AddTransient<IDatabaseContext, DatabaseContext>()
                .AddTransient<DatabaseContext, DatabaseContext>();

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                EnsureDatabaseReady(scope.ServiceProvider);
            }

            return services
                .AddTransient<IScenarioService, ScenarioService>()
                .AddTransient<IScenarioConsequenceExpressionBuilder, ScenarioConsequenceExpressionBuilder>()
                .AddTransient<IScenarioParsingService, ScenarioParsingService>()
                .AddHostedService<ScenarioEventService>()
                .AddSingleton<IScenarioEventService, ScenarioEventService>()
                .AddTransient<IScenarioEventPropagator, ScenarioEventPropagator>()
                .AddSingleton<SingletonCacheService>()
                .AddTransient<ISerializationService, SerializationService>()
                .AddDelegates()
                .AddExpressionBuilding(domainAssembly);
        }

        public static IApplicationBuilder UseScenario(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                EnsureDatabaseReady(scope.ServiceProvider);
            }

            return app;
        }

        private static IServiceCollection AddDelegates(this IServiceCollection services)
        {
            return services.AddSingleton<PredicateWhereClauseResolver>(provider =>
            {
                var singletonCacheService = provider.GetRequiredService<SingletonCacheService>();
                return (whereClauseKey) => singletonCacheService.PredicateTypeLookupTable.TryGetValue(whereClauseKey, out Type type) ? type : null;
            });
        }

        private static void EnsureDatabaseReady(IServiceProvider serviceProvider)
        {
            var databaseContext = serviceProvider.GetRequiredService<DatabaseContext>();

            var infrastructure = (databaseContext as IInfrastructure<IServiceProvider>).Instance;

            var migrationsAssembly = infrastructure.GetRequiredService<IMigrationsAssembly>();
            var differ = infrastructure.GetRequiredService<IMigrationsModelDiffer>();

            var snapshot = migrationsAssembly.ModelSnapshot.Model as IRelationalModel;

            if (differ.GetDifferences(snapshot, databaseContext.Model as IRelationalModel).Any())
            {
                throw new InvalidOperationException("There are differences between the current database model and the most recent migration.");
            }

            databaseContext.Database.Migrate();
        }
    }
}
