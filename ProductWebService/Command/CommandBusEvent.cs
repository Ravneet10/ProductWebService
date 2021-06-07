using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductWebService.Command
{
    public class CommandBusEvent
    {
        public CommandBusEventType Type { get; set; }
        public Command Command { get; set; }
        public IEnumerable<Exception> Exceptions { get; set; }

        public CommandBusEvent(CommandBusEventType eventType, Command command = null, IEnumerable<Exception> exceptions = null)
        {
            Type = eventType;
            Command = command;
            Exceptions = exceptions;
        }

        /// <summary>
        /// Command bus event notification type.
        /// </summary>
        public enum CommandBusEventType
        {
            Pending,
            Handling,
            Succeeded,
            Failed
        }
    }
}
