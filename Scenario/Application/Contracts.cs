using System;
namespace Scenario.Application
{
    public record ScenarioDto(string Id, string Title, string Scenario, DateTime Created, bool Active);

    public record ScenarioCreateDto(string Title, string Scenario);

    public record ScenarioCreateResult(string Id, string Title, DateTime Created, bool Active);

}
