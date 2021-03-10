using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace RegisterModuleLab
{
    public class Program
    {
        // Methods
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
                .CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<Autofac.ContainerBuilder>((containerBuilder) =>
                {
                    // Module
                    containerBuilder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());
                })
                .ConfigureServices(services =>
                {
                    // ConsoleService
                    //services.AddHostedService<ConsoleService>();
                })
            ;
        }


        // Class
        public class ConsoleService : BackgroundService
        {
            // Fields
            private readonly SettingContext _settingContext = null;


            // Constructors
            public ConsoleService(SettingContext settingContext)
            {
                #region Contracts

                if (settingContext == null) throw new ArgumentException(nameof(settingContext));

                #endregion

                // Default
                _settingContext = settingContext;
            }


            // Methods
            protected override Task ExecuteAsync(CancellationToken stoppingToken)
            {
                return Task.Run(() =>
                {
                    // Execute
                    _settingContext.Execute();
                });
            }
        }

        public class SettingContext
        {
            // Constructors
            public SettingContext()
            {
                // Display
                Console.WriteLine("SettingContext Created");
            }


            // Methods
            public void Execute()
            {
                // Execute
                Console.WriteLine("Hello World!");
            }
        }

        public class SettingContextModule : Autofac.Module
        {
            // Methods
            protected override void Load(ContainerBuilder builder)
            {
                #region Contracts

                if (builder == null) throw new ArgumentException(nameof(builder));

                #endregion

                // SettingContext
                {
                    // Register
                    builder.RegisterType<SettingContext>().As<SettingContext>()

                    // Start
                    .OnActivated(handler =>
                    {

                    })

                    // Lifetime
                    .AutoActivate().SingleInstance();
                }
            }
        }
    }
}