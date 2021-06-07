using Autofac;
using Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ProductWebService.Command;
using ProductWebService.Handlers;
using ProductWebService.Controllers;

namespace ProductWebService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var container = CreateContainer();
            services.AddTransient<ICommandBus, CommandBus>();
            services.AddDbContext<ProductContext>(opts =>
            opts.UseInMemoryDatabase("userDB"));
            services.AddScoped<ProductContext>();
            services.AddMvc().AddControllersAsServices();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(Startup));
        }
        private void RegisterCommandBus(ContainerBuilder builder)
        {
            var serviceName = Assembly.GetExecutingAssembly().GetName().Name;
            var commandHandlersAssembly = typeof(CreateProductCommandHandler).Assembly;
            var commandBusModule = new QueueCommandBusModule(serviceName, commandHandlersAssembly);
            builder.RegisterModule(commandBusModule);
        }
        public IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();
            RegisterCommandBus(builder);
            builder
                .RegisterType<ProductContext>()
                .As<IProductContext>()
                .WithParameter(new TypedParameter(typeof(Action<string>), ""))
                .WithParameter(new TypedParameter(typeof(string), ""));

            return builder.Build();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
