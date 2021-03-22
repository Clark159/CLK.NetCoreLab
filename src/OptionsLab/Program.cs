using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OptionsLab
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
                .ConfigureServices((hostContext, services) =>
                {
                    // ProgramOptions
                    {
                        // Configure
                        services.Configure<ProgramOptions>((options) =>
                        {
                            options.Value001 = "Config001";
                        });

                        // IConfigureOptions
                        services.AddSingleton<IConfigureOptions<ProgramOptions>>
                        (
                            new ConfigureNamedOptions<ProgramOptions>(Options.DefaultName, (options) =>
                            {
                                options.Value002 = "Config002";
                            })
                        );

                        // IConfiguration
                        services.Configure<ProgramOptions>
                        (
                            hostContext.Configuration.GetSection("ProgramOptions")
                        );

                        // AddOptions
                        services.AddOptions<ProgramOptions>().Configure<IHostEnvironment>((options, hostEnvironment) =>
                        {
                            options.Value005 = hostEnvironment.ApplicationName;
                        });
                    }

                    // ProgramService
                    services.AddHostedService<ProgramService>();
                })
                .ConfigureContainer<Autofac.ContainerBuilder>((container) =>
                {
                    // ProgramOptions
                    {
                        // IConfigureOptions
                        container.RegisterInstance<IConfigureOptions<ProgramOptions>>
                        (
                            new ConfigureNamedOptions<ProgramOptions>(Options.DefaultName, (options) =>
                            {
                                options.Value004 = "Config004";
                            })
                        );
                    }
                });


        // Class
        public class ProgramService : BackgroundService
        {
            // Fields
            private readonly IOptions<ProgramOptions> _options = null;


            // Constructors
            public ProgramService(IOptions<ProgramOptions> options)
            {
                #region Contracts

                if (options == null) throw new ArgumentException(nameof(options));

                #endregion

                // Default
                _options = options;
            }


            // Methods
            protected override Task ExecuteAsync(CancellationToken stoppingToken)
            {
                return Task.Run(() =>
                {
                    // Display
                    Console.WriteLine($"_options.Value001={_options.Value?.Value001}");
                    Console.WriteLine($"_options.Value002={_options.Value?.Value002}");
                    Console.WriteLine($"_options.Value003={_options.Value?.Value003}");
                    Console.WriteLine($"_options.Value004={_options.Value?.Value004}");
                    Console.WriteLine($"_options.Value005={_options.Value?.Value005}");
                });
            }
        }

        public class ProgramOptions
        {
            // Properties
            public string Value001 { get; set; } = "Default001";

            public string Value002 { get; set; } = "Default002";

            public string Value003 { get; set; } = "Default003";

            public string Value004 { get; set; } = "Default004";

            public string Value005 { get; set; } = "Default005";
        }
    }
}