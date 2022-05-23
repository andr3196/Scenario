using System;
using MediatR;
using Scenario.Contracts.Commands;

namespace Scenario.Application.CommandHandlers.Update;

public interface IUpdateScenarioCommandHandler : IRequestHandler<UpdateScenarioCommand, Guid>
{
    
}