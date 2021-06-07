using Autofac;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Threading.Tasks;

namespace ProductWebService.Command
{
    public class CommandBus : ICommandBus, IDisposable
    {
        public CommandBus(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        /// <summary>
        /// Pass command directly to handler (no queueing)
        /// Implementers should override this method to handle receiving of commands.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        public virtual async Task<string> ExecuteAsync<TCommand>(TCommand command) where TCommand : Command
        {
            Task allCommandHandlersTask = null;
            try
            {
                var stopwatch = Stopwatch.StartNew();

                var commandHandlerTasks = GetCommandHandlerInvocationTasks(command);
                allCommandHandlersTask = Task.WhenAll(commandHandlerTasks);
                await allCommandHandlersTask;

                _logger.Debug("Command {@Command} handled successfully", command);

                stopwatch.Stop();
                _logger.Debug("Command handled duration {Milliseconds}ms", stopwatch.ElapsedMilliseconds);
            }
            catch (Exception)
            {
                const string errorMessage = "Command handling error.";
                _logger.Error(allCommandHandlersTask?.Exception, errorMessage);
            }
            finally
            {
                _logger.Debug("All handlers invoked for command {@Command}", command);
            }

            return command.CommandId;
        }


        /// <summary>
        /// Pass command directly to handler (no queueing)
        /// Implementers should override this method to handle receiving of commands.
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        public virtual async Task<IList<string>> ExecuteAsync<TCommand>(List<TCommand> commands) where TCommand : Command
        {
            if (commands == null) throw new ArgumentNullException(nameof(commands));

            List<string> commandIds = new List<string>();
            foreach (TCommand command in commands)
            {
                commandIds.Add(await ExecuteAsync(command));
            }

            return commandIds;
        }

        protected IEnumerable<Task> GetCommandHandlerInvocationTasks(object command)
        {
            // Support multiple handlers per command by resolving enumeration relationship type
            // IEnumerable<ICommandHandler<TCommand>> to find all command handler implementations.

            var commandType = command.GetType();
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);
            var enumerableHandlerType = typeof(IEnumerable<>).MakeGenericType(handlerType);
            var handlerList = ((IEnumerable<object>)_lifetimeScope.Resolve(enumerableHandlerType)).ToList();

            // fix: exclude contravariant implementations, i.e. handlers implementing ICommandHandler<Base>
            handlerList = handlerList.Where(h => h.GetType().GetTypeInfo().ImplementedInterfaces.Contains(handlerType)).ToList();

            if (!handlerList.Any()) { throw new Exception($"No command handlers registered for {commandType.Name}"); }

            var handlerNames = handlerList.Select(h => h.GetType().FullName).ToList();
            _logger.Debug("Found {HandlerCount} registered handlers for {@Command} {@HandlerTypes}", handlerList.Count, command, handlerNames);

            var handlerTasks = handlerList.Select(commandHandler =>
                (Task)handlerType
                    .GetTypeInfo()
                    .GetDeclaredMethod(nameof(ICommandHandler<Command>.ExecuteAsync))
                    .Invoke(commandHandler, new[] { command })).ToList();

            return handlerTasks;
        }

        private readonly Subject<CommandBusEvent> _eventStream = new Subject<CommandBusEvent>();
        protected readonly ILifetimeScope _lifetimeScope;
        protected ILogger _logger;

        public virtual string CommandBusName => nameof(CommandBus);

        public virtual void Dispose()
        {
            _logger.Debug($"{CommandBusName} disposed");
        }
    }
}
