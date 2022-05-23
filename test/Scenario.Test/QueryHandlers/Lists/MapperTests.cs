using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Scenario.Application.QueryHandlers.Lists;
using Scenario.Contracts;
using Scenario.Domain.Models;
using Scenario.Domain.Models.Clauses;
using Scenario.Project.Test;
using Scenario.TestDataBuilder.Domain;
using Xunit;

namespace Scenario.Test.QueryHandlers.Lists;


public class MapperTests : IClassFixture<WebApplicationFactory<Scenario.Project.Test.Program>>
{
    private readonly WebApplicationFactory<Program> factory;

    public MapperTests(WebApplicationFactory<Project.Test.Program> factory)
    {
        this.factory = factory;
    }
    [Fact]
    public void ScenarioDefinitionMapper_ShouldMapAllProperties()
    {
        var mapper = factory.Services.GetRequiredService<IMapper>();
        var builder = factory.Services.GetRequiredService<ScenarioDefinitionBuilder>();
        var operatorValue = new UnaryOperatorClause(new ValueClause("test", "string", "test"), "operator");
        var condition =
            new UnaryPredicateClause(operatorValue);
        
        var scenario = builder
            .WithTitle("Title1").IsActive().WithCondition(condition).WithDummyConsequence().Build();
        

        var dto = mapper.Map<ScenarioDefinitionDto>(scenario);
        
        Assert.NotNull(dto);
        Assert.Equal(scenario.Id, dto.Id);
        Assert.Equal(scenario.Title, dto.Title);
        Assert.True(dto.IsActive);
        AssertConditionsAreEqual(operatorValue, dto.Condition);
    }

    private static void AssertConditionsAreEqual(UnaryOperatorClause condition, IPredicateClause dtoCondition)
    {
        Assert.NotNull(dtoCondition);
        var dtoPredicate = dtoCondition as UnaryPredicateClause;
        Assert.NotNull(dtoPredicate);
        var dtoOperator = dtoPredicate!.Value as UnaryOperatorClause;
        Assert.NotNull(dtoOperator);
        Assert.Equal(condition.OperatorKey, dtoOperator!.OperatorKey);
        Assert.Equal(condition.Value, dtoOperator.Value);
    }
}