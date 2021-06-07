using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ProductWebService.Command
{
    public class CommandBusModule : Autofac.Module
    {
        private readonly List<Assembly> _handlerAssemblies = new List<Assembly>();
        private readonly List<Type> _handlerTypes = new List<Type>();

        public CommandBusModule() { }

        /// <summary>
        /// Register the given <see cref="ICommandHandler{TCommand}"/> types.
        /// </summary>
        public CommandBusModule(params Type[] handlerTypes)
        {
            AddHandlerTypes(handlerTypes);
        }

        /// <summary>
        /// Scan the given assemblies for <see cref="ICommandHandler{TCommand}"/> types to register.
        /// </summary>
        public CommandBusModule(params Assembly[] handlerAssemblies)
        {
            AddHandlerAssemblies(handlerAssemblies);
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (_handlerAssemblies != null && _handlerAssemblies.Any())
            {
                RegisterHandlersFromAssemblies(builder, _handlerAssemblies);
            }

            if (_handlerTypes != null && _handlerTypes.Any())
            {
                RegisterHandlerTypes(builder, _handlerTypes);
            }

            base.Load(builder);
        }

        public void AddHandlerAssemblies(params Assembly[] handlerAssemblies)
        {
            if (handlerAssemblies == null || !handlerAssemblies.Any())
            {
                throw new ArgumentException("", nameof(handlerAssemblies));
            }
            _handlerAssemblies.AddRange(handlerAssemblies);
        }

        public void AddHandlerTypes(params Type[] handlerTypes)
        {
            if (handlerTypes == null || !handlerTypes.Any())
            {
                throw new ArgumentException("", nameof(handlerTypes));
            }
            _handlerTypes.AddRange(handlerTypes);
        }

        private void RegisterHandlerTypes(ContainerBuilder builder, IEnumerable<Type> handlerTypes)
        {
            var handlerTypesList = handlerTypes?.ToList();

            if (handlerTypesList == null || !handlerTypesList.Any()) { throw new ArgumentException("", nameof(handlerTypes)); }

            var singletonHandlerTypes = handlerTypesList.Where(t => typeof(ISingletonCommandHandler).IsAssignableFrom(t)).ToArray();

            IEnumerable<Service> GetCommandHandlerServices(Type type) => type.GetTypeInfo()
                .ImplementedInterfaces.Where(t => t.IsClosedTypeOf(typeof(ICommandHandler<>)))
                .Select(t => (Service)new TypedService(t));

            builder.RegisterTypes(handlerTypesList.Except(singletonHandlerTypes).ToArray())
                .InstancePerDependency()
                .As(type => GetCommandHandlerServices(type));

            builder.RegisterTypes(singletonHandlerTypes.ToArray())
                .SingleInstance()
                .As(type => GetCommandHandlerServices(type));
        }

        private void RegisterHandlersFromAssemblies(ContainerBuilder builder, IEnumerable<Assembly> assemblies)
        {
            var assembliesList = assemblies?.ToList();

            if (assembliesList == null || !assembliesList.Any()) { throw new ArgumentException("", nameof(assemblies)); }

            bool IsCommandHandler(TypeInfo type) =>
                type.IsClass &&
                type.IsPublic &&
                !type.IsAbstract
                && type.IsClosedTypeOf(typeof(ICommandHandler<>))
                && type.Name.EndsWith("CommandHandler");

            var handlerTypes = assembliesList
                .SelectMany(a => a.DefinedTypes)
                .Where(IsCommandHandler)
                .Select(t => t.AsType())
                .ToArray();

            if (!handlerTypes.Any())
            {
                throw new Exception("No command handlers found in assemblies.");
            }

            RegisterHandlerTypes(builder, handlerTypes);
        }
    }

}
