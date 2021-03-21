using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace NamedServiceLab
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
                .ConfigureServices((services) =>
                {
                    // ProgramService
                    services.AddHostedService<ProgramService>();
                })
                .ConfigureAppConfiguration((hostContext, configuration) =>
                {
                    // Memory
                    configuration.AddInMemoryCollection(new Dictionary<string, string>()
                    {
                        {"SettingContextName","AAA"}
                    });
                })
                .ConfigureContainer<Autofac.ContainerBuilder>((container) =>
                {
                    // Register
                    container.Register<SettingContext>(componentContext =>
                    {
                        // Configuration
                        var configuration = componentContext.Resolve<IConfiguration>();
                        if (configuration == null) throw new InvalidOperationException($"{nameof(configuration)}=null");

                        // ServiceName
                        var serviceName = configuration.GetValue<string>("SettingContextName");
                        if (string.IsNullOrEmpty(serviceName) == true) throw new InvalidOperationException($"{nameof(serviceName)}=null");

                        // Resolve
                        return componentContext.ResolveNamed<SettingContext>(serviceName);
                    });
                    container.RegisterType<SettingContextAAA>().Named<SettingContext>("AAA");
                    container.RegisterType<SettingContextBBB>().Named<SettingContext>("BBB");
                });


        // Class
        public class ProgramService : BackgroundService
        {
            // Fields
            private readonly SettingContext _settingContext = null;


            // Constructors
            public ProgramService(SettingContext settingContext)
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


        public interface SettingContext
        {
            // Methods
            string GetValue();
        }

        public class SettingContextAAA : SettingContext
        {
            // Methods
            public string GetValue()
            {
                // Return
                return "Hello World! : AAA";
            }
        }

        public class SettingContextBBB : SettingContext
        {
            // Methods
            public string GetValue()
            {
                // Return
                return "Hello World! : BBB";
            }
        }
    }
}