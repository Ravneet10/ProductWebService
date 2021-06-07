using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ProductWebService.Command
{
    public class QueueCommandBusModule : CommandBusModule
    {
        private readonly string _serviceName;

        public QueueCommandBusModule(string serviceName, params Assembly[] handlerAssemblies) : base(handlerAssemblies)
        {
            _serviceName = serviceName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<CommandBus>().As<ICommandBus>()
            .InstancePerLifetimeScope();
           
        }
    }

}
