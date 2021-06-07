using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductWebService.Command
{
    public interface ICommandBus
    {
        Task<string> ExecuteAsync<TCommand>(TCommand command) where TCommand : Command;
        Task<IList<string>> ExecuteAsync<TCommand>(List<TCommand> commands) where TCommand : Command;
    }
}
