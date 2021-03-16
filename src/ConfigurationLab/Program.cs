using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ConfigurationLab
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
                .ConfigureAppConfiguration((hostContext, configuration) =>
                {
                    // File
                    configuration.AddJsonFile(Path.Combine(AppContext.BaseDirectory, "FileSetting.json"));

                    // Memory
                    configuration.AddInMemoryCollection(new Dictionary<string, string>()
                    {
                        {"service02:Id","Memory_Id02"},
                        {"service02:Name","Memory_Name02"},
                        {"Setting02","Memory_Setting02"}
                    });
                })
                .ConfigureServices((services) =>
                {
                    // ProgramService
                    services.AddHostedService<ProgramService>();
                });


        // Class
        public class ProgramService : BackgroundService
        {
            // Fields
            private readonly IConfiguration _configuration = null;


            // Constructors
            public ProgramService(IConfiguration configuration)
            {
                #region Contracts

                if (configuration == null) throw new ArgumentException(nameof(configuration));

                #endregion

                // Default
                _configuration = configuration;
            }


            // Methods
            protected override Task ExecuteAsync(CancellationToken stoppingToken)
            {
                return Task.Run(() =>
                {
                    // File
                    {
                        // Service01                       
                        var service01 = _configuration.Bind<ServiceSetting>("service01");
                        Console.WriteLine($"service01.Id  = {service01.Id}");
                        Console.WriteLine($"service01.Name= {service01.Name}");

                        // Setting01
                        var setting01 = _configuration.GetValue<string>("Setting01");
                        Console.WriteLine($"setting01     = {setting01}");
                    }
                    Console.WriteLine();

                    // Memory
                    {
                        // Service02                       
                        var service02 = _configuration.Bind<ServiceSetting>("service02");
                        Console.WriteLine($"service02.Id  = {service02.Id}");
                        Console.WriteLine($"service02.Name= {service02.Name}");

                        // Setting02
                        var setting02 = _configuration.GetValue<string>("Setting02");
                        Console.WriteLine($"setting02     = {setting02}");
                    }
                    Console.WriteLine();
                });
            }
        }

        public class ServiceSetting
        {
            // Properties
            public string Id { get; set; }

            public string Name { get; set; }
        }
    }
}