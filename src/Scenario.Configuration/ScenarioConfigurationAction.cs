using Microsoft.AspNetCore.Builder;

namespace Scenario.Configuration;

public delegate void ScenarioConfigurationAction(IApplicationBuilder app);