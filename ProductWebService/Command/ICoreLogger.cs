using Serilog;

namespace ProductWebService.Command
{
    public interface ICoreLogger
    {
        ILogger Serilog { get; }
    }
}