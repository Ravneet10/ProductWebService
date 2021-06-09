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
using Autofac.Extensions.DependencyInjection;

namespace ProductWebService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public ILifetimeScope AutofacContainer { get; private set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ICommandBus, CommandBus>();
            services.AddDbContext<ProductContext>(opts =>
            opts.UseInMemoryDatabase("userDB"));
            services.AddScoped<ProductContext>();
            services.AddMvc().AddControllersAsServices();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(Startup));
            services.AddOptions();
        }
        private void RegisterCommandBus(ContainerBuilder builder)
        {
            var serviceName = Assembly.GetExecutingAssembly().GetName().Name;
            var commandHandlersAssembly = typeof(CreateProductCommandHandler).Assembly;
            var commandBusModule = new QueueCommandBusModule(serviceName, commandHandlersAssembly);
            builder.RegisterModule(commandBusModule);
        }
        public void CreateContainer(ContainerBuilder builder)
        {
            RegisterCommandBus(builder);
            builder
                .RegisterType<ProductContext>()
                .As<IProductContext>()
                .WithParameter(new TypedParameter(typeof(Action<string>), ""))
                .WithParameter(new TypedParameter(typeof(string), ""));

        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory
            // for you.
            CreateContainer(builder);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();
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
