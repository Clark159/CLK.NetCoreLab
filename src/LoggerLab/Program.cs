using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LoggerLab
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
                    // ProgramService
                    services.AddHostedService<ProgramService>();
                });


        // Class
        public class ProgramService : BackgroundService
        {
            // Fields
            private readonly ILogger _logger;


            // Constructors
            public ProgramService(ILogger<ProgramService> logger)
            {
                #region Contracts

                if (logger == null) throw new ArgumentException(nameof(logger));

                #endregion

                // Default
                _logger = logger;
            }


            // Methods
            protected override Task ExecuteAsync(CancellationToken stoppingToken)
            {
                return Task.Run(() =>
                {
                    // Logger-Message
                    Console.WriteLine("=====Logger-Message=====");
                    _logger.LogTrace("Trace Message");
                    _logger.LogDebug("Debug Message");
                    _logger.LogInformation("Information Message");
                    _logger.LogWarning("Warning Message");
                    _logger.LogError("Error Message");
                    _logger.LogCritical("Critical Message");
                    Console.WriteLine("\n");

                    // Logger-Exception
                    Console.WriteLine("=====Logger-Exception=====");
                    _logger.LogTrace(this.GetException("Trace Exception"), "Trace Message");
                    _logger.LogDebug(this.GetException("Debug Exception"), "Debug Message");
                    _logger.LogInformation(this.GetException("Information Exception"), "Information Message");
                    _logger.LogWarning(this.GetException("Warning Exception"), "Warning Message");
                    _logger.LogError(this.GetException("Error Exception"), "Error Message");
                    _logger.LogCritical(this.GetException("Critical Exception"), "Critical Message");
                    Console.WriteLine("\n");
                });
            }

            private Exception GetException(string message)
            {
                #region Contracts

                if (string.IsNullOrEmpty(message) == true) throw new ArgumentException(nameof(message));

                #endregion

                try
                {
                    // Throw
                    throw new Exception(message);
                }
                catch (Exception ex)
                {
                    // Return
                    return ex;
                }
            }
        }
    }
}