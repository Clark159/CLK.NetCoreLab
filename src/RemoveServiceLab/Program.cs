using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RemoveServiceLab
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
                .ConfigureServices((services) =>
                {
                    // RemoveService
                    services.RemoveService<ConsoleLoggerProvider>();

                    // ProgramService
                    services.AddHostedService<ProgramService>();
                });


        // Class
        public class ProgramService : BackgroundService
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