using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace WithParameterLab
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
                .ConfigureContainer<Autofac.ContainerBuilder>((container) =>
                {
                    // SettingContext
                    {
                        // ParameterList
                        var parameterList = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        parameterList.Add("AAA", "Data001");
                        parameterList.Add("BBB", "123");

                        // Register
                        container.RegisterType<SettingContext>().As<SettingContext>().WithParameter
                        (
                            // ParameterSelector
                            (parameterInfo, componentContext) =>
                            {
                                return parameterList.ContainsKey(parameterInfo.Name);
                            },

                            // ValueProvider
                            (parameterInfo, componentContext) =>
                            {
                                return Convert.ChangeType(parameterList[parameterInfo.Name], parameterInfo.ParameterType);
                            }
                        );
                    }
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

        public class SettingContext
        {
            // Constructors
            public SettingContext(string aaa, int bbb)
            {
                // Display
                Console.WriteLine($"aaa={aaa}");
                Console.WriteLine($"bbb={bbb}");
            }

            // Methods
            public string GetValue()
            {
                // Return
                return "Hello World!";
            }
        }
    }
}