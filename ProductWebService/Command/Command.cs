using System;

namespace ProductWebService.Command
{
    public abstract class Command
    {
        public string CommandId { get; set; }

        protected Command()
        {
            CommandId = $"command-{Guid.NewGuid().ToString()}";
        }

        protected Command(string commandId)
        {
            if (string.IsNullOrEmpty(commandId) || commandId.Length > 128)
            {
                throw new ArgumentException("", nameof(commandId));
            }

            CommandId = commandId;
        }

    }
}
