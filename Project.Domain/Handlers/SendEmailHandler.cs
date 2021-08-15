using System;
using System.Threading;
using System.Threading.Tasks;
using Scenario.Domain.Modeling.Attributes;

namespace Project.Domain.Handlers
{
    [ScenarioConsequence(label:"send email", parametersType:typeof(SendEmailCommand))]
    public class SendEmailHandler : ICommandHandler<SendEmailCommand>
    {
        public Task HandleAsync(SendEmailCommand command, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"Sending email to {command.Receiver} on {command.SendTime}:\nTitle: {command.Title}\nMessage:{command.Message}");
            return Task.CompletedTask;
        }
    }
}
