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
            #region Contracts

            if (logger == null) throw new ArgumentException(nameof(logger));
            if (hostEnvironment == null) throw new ArgumentException(nameof(hostEnvironment));

            #endregion

            // Execute
            logger.LogWarning($"Clark Message");
            logger.LogWarning(hostEnvironment.ContentRootPath);
        }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((services) =>
                {
                    services.AddConsoleService<Program>();
                });
    }
}
