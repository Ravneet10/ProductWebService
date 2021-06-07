using System.Threading.Tasks;

namespace ProductWebService.Command
{
    public interface ISingletonCommandHandler { }
    public interface ICommandHandler<in TCommand> where TCommand : Command
    {
        Task ExecuteAsync(TCommand command);
    }
}
