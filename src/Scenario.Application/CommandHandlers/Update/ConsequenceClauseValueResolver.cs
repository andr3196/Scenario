using System;
using AutoMapper;
using Scenario.Contracts.Commands;
using Scenario.Domain.Models;
using Scenario.Domain.Serialization.JsonConvertion;

namespace Scenario.Application.CommandHandlers.Update
{
    public class ConsequenceClauseValueResolver : IValueResolver<UpdateScenarioCommand, ScenarioDefinition, string>
    {
        private readonly IConsequenceClauseConverter converter;

        public ConsequenceClauseValueResolver(IConsequenceClauseConverter converter)
        {
            this.converter = converter;
        }
        
        public string Resolve(UpdateScenarioCommand source, ScenarioDefinition destination, string destMember,
            ResolutionContext context)
        {
            return converter.Serialize(source.Consequence);
        }
    }
}