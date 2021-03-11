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
                    // Json
                    configuration.AddJsonFile(Path.Combine(AppContext.BaseDirectory, "FileSetting.json"));

                    // Memory
                    configuration.AddInMemoryCollection(new Dictionary<string, string>()
                    {
                        {"MemorySeting","Jane"}
                    });
                })
                .ConfigureServices((services) =>
                {
                    // ConsoleService
                    services.AddHostedService<ConsoleService>();
                });


        // Class
        public class ConsoleService : BackgroundService
        {
            // Methods
            protected override Task ExecuteAsync(CancellationToken stoppingToken)
            {
                return Task.Run(() =>
                {
                    // Execute
                    Console.WriteLine("Hello World!");
                });
            }
        }
    }
}