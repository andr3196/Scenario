using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scenario.Api.Test.Extensions;
using Scenario.Contracts;
using Scenario.Contracts.Commands;
using Scenario.Core;
using Scenario.Domain.Models;
using Scenario.Domain.Models.Clauses;
using Scenario.Domain.Serialization.JsonConvertion;
using Scenario.EntityFramework.Test;
using Scenario.EntityFrameworkCore;
using Scenario.TestDataBuilder.Domain;
using Xunit;

namespace Scenario.Api.Test;

public class ScenarioApiControllerTests : IClassFixture<WebApplicationFactory<Project.Test.Program>>, IClassFixture<DatabaseFixture<DatabaseContext>>
{
    private readonly WebApplicationFactory<Project.Test.Program> factory;

    private readonly JsonSerializerOptions options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new PredicateClauseConverter() },
    };

    public ScenarioApiControllerTests(WebApplicationFactory<Project.Test.Program> factory)
    {
        this.factory = factory;
    }
    
    [Fact]
    public async Task GetScenariosList_ShouldBeReachable()
    {
        using var client = factory.CreateDefaultClient();

        var response = await client.GetAsync("/Scenario");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task GetScenariosList_ShouldReturnScenarioDefinitionsFromDatabase()
    {
        using var client = factory.CreateDefaultClient();
        using var scope = factory.Services.CreateScope();
        await using var unbuilder = await factory.Services.GetRequiredService<ScenarioDefinitionBuilder>()
            .WithTitle("Scenario1")
            .IsActive()
            .WithDummyCondition()
            .WithDummyConsequence()
            .Repeat()
            .WithTitle("Scenario2")
            .IsActive(false)
            .ExistsInDatabaseAsync(scope.ServiceProvider.GetRequiredService<DatabaseContext>());
        var scenarios = unbuilder.Entities;
                
        var retrievedScenarios = await client.GetAsync<ScenarioDefinitionDto[]>("/Scenario");
        retrievedScenarios.Should().NotBeNull();
        var sortedScenarios = retrievedScenarios!.OrderBy(dto => dto.Title).ToList();
        sortedScenarios.Should().Fulfill(
            firstScenario => new List<Action>
            {
                () => firstScenario.IsActive.Should().BeTrue(),
                () => firstScenario.Title.Should().Be(scenarios[0].Title),
            },
            secondScenario => new List<Action>
            {
                () => secondScenario.IsActive.Should().BeFalse(),
                () => secondScenario.Title.Should().Be(scenarios[1].Title)
            });
    }
    
    [Fact]
    public async Task GetScenariosMetadataList_ShouldReturnScenarioDefinitionMetadatasFromDatabase()
    {
        using var client = factory.CreateDefaultClient();
        using var scope = factory.Services.CreateScope();
        await using var unbuilder = await factory.Services.GetRequiredService<ScenarioDefinitionBuilder>()
            .WithTitle("Scenario1")
            .IsActive()
            .WithDummyCondition()
            .WithDummyConsequence()
            .Repeat()
            .WithTitle("Scenario2")
            .IsActive(false)
            .ExistsInDatabaseAsync(scope.ServiceProvider.GetRequiredService<DatabaseContext>());
        var scenarios = unbuilder.Entities;
                
        var retrievedScenarios = await client.GetAsync<ScenarioMetadataDto[]>("/Scenario/metadata");
        retrievedScenarios.Should().NotBeNull();
        var sortedScenarios = retrievedScenarios!.OrderBy(dto => dto.Title).ToList();
        sortedScenarios.Should().Fulfill(
            firstScenario => new List<Action>
            {
                () => firstScenario.IsActive.Should().BeTrue(),
                () => firstScenario.Title.Should().Be(scenarios[0].Title),
            },
            secondScenario => new List<Action>
            {
                () => secondScenario.IsActive.Should().BeFalse(),
                () => secondScenario.Title.Should().Be(scenarios[1].Title)
            });
    }
    
    [Fact]
    public async Task CreateScenario_ShouldAddScenarioToDatabase()
    {
        using var client = factory.CreateDefaultClient();
        using var scope = factory.Services.CreateScope();
        var createCommand = new CreateScenarioCommand
        {
            Id = Guid.Parse("40f23887-9ab7-4e9d-9d79-459e8653c202"),
            Condition = new UnaryPredicateClause(new UnaryOperatorClause(new ValueClause("type", "valueType", "value"),"not null")),
            Consequence = new ConsequenceClause("consequnce-1", new Dictionary<string, ValueClause>()),
            Title = "Scenario1",
            IsActive = true,
        };

        var id = await client.PostAsync<CreateScenarioCommand, Guid>("/Scenario", createCommand);
        Assert.NotEqual(Guid.Empty, id);

        var source = scope.ServiceProvider.GetRequiredService<ISource<ScenarioDefinition>>();
        var scenario = await source.SingleOrDefaultAsync(s => s.Id == id);
        scenario.Should().NotBeNull();
        scenario!.Title.Should().Be(createCommand.Title);
        var conditionJson = JsonSerializer.Serialize(createCommand.Condition, options);
        scenario.ConditionJson.Should().Be(conditionJson);
        var consequenceJson = JsonSerializer.Serialize(createCommand.Consequence, options);
        scenario.ConsequenceJson.Should().Be(consequenceJson);
        scenario.IsActive.Should().Be(createCommand.IsActive);
    }
    
    [Fact]
    public async Task UpdateScenario_ShouldUpdateScenarioInDatabase()
    {
        using var client = factory.CreateDefaultClient();
        using var scope = factory.Services.CreateScope();
        await using var unbuilder = await factory.Services.GetRequiredService<ScenarioDefinitionBuilder>()
            .WithTitle("Scenario1")
            .IsActive()
            .WithDummyCondition()
            .WithDummyConsequence()
            .ExistsInDatabaseAsync(scope.ServiceProvider.GetRequiredService<DatabaseContext>());
        var scenarios = unbuilder.Entities;

        var updateCommand = new UpdateScenarioCommand
        {
            Id = scenarios[0].Id,
            Condition = new UnaryPredicateClause(new UnaryOperatorClause(new ValueClause("othertype", "valueType", "othervalue"),"should be null")),
            Consequence = new ConsequenceClause("consequnce-2", new Dictionary<string, ValueClause>()),
            Title = "Scenario2",
            IsActive = false,
        };
        
        var id = await client.PatchAsync<UpdateScenarioCommand, Guid>("/Scenario", updateCommand);
        Assert.NotEqual(Guid.Empty, id);

        var source = scope.ServiceProvider.GetRequiredService<ISource<ScenarioDefinition>>();
        var scenario = await source.SingleOrDefaultAsync(s => s.Id == id);
        scenario.Should().NotBeNull();
        scenario!.Title.Should().Be(updateCommand.Title);
        var conditionJson = JsonSerializer.Serialize(updateCommand.Condition, options);
        scenario.ConditionJson.Should().Be(conditionJson);
        var consequenceJson = JsonSerializer.Serialize(updateCommand.Consequence, options);
        scenario.ConsequenceJson.Should().Be(consequenceJson);
        scenario.IsActive.Should().Be(updateCommand.IsActive);
    }
}