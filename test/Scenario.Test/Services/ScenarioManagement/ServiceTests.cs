using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Moq;
using Scenario.Domain.Services.ScenarioManagement;
using Scenario.Domain.Shared.TypeHandling;
using Scenario.Test.Services.ExpressionBuilding.Mocks;
using Xunit;

namespace Scenario.Test.Services.ScenarioManagement
{
    public class ServiceTests
    {
        [Fact]
        public void ShouldBuildCommandForWithSimpleProperties()
        {
            // Arrange

            var service = new ScenarioCreator();

            // Act
            
            

            // Assert

        }
    }
}
