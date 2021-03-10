using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
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
                .ConfigureServices(services =>
                {
                    // ConsoleService
                    services.AddHostedService<ConsoleService>();
                })
            ;
        }


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