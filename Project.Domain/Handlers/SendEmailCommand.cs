using System;
namespace Project.Domain.Handlers
{
    public class SendEmailCommand : ICommand
    {
        public string Receiver { get; set; }

        public DateTime SendTime { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }
    }
}
