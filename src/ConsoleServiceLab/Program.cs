using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace ConsoleServiceLab
{
    public class Program
    {
        // Methods
        public static void Run(ILogger<Program> logger, IHostEnvironment hostEnvironment)
        {
            logger.LogWarning($"Clark Message");
            logger.LogWarning(hostEnvironment.ContentRootPath);
        }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddConsoleService<Program>();
                })
            ;
    }
}
