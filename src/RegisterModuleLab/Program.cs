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

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<Autofac.ContainerBuilder>((container) =>
                {
                    // Module
                    container.RegisterAssemblyModules(Assembly.GetEntryAssembly());
                })
                .ConfigureServices((services) =>
                {
                    // ConsoleService
                    services.AddHostedService<ConsoleService>();
                });


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
                    Console.WriteLine(_settingContext.GetValue());
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
            public string GetValue()
            {
                // Return
                return "Hello World!";
            }
        }

        public class SettingContextModule : Autofac.Module
        {
            // Methods
            protected override void Load(ContainerBuilder container)
            {
                #region Contracts

                if (container == null) throw new ArgumentException(nameof(container));

                #endregion

                // SettingContext
                {
                    // Register
                    container.RegisterType<SettingContext>().As<SettingContext>();
                }
            }
        }
    }
}