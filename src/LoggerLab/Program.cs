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

            private readonly ILoggerFactory _loggerFactory = null;


            // Constructors
            public ProgramService(ILogger<ProgramService> logger, ILoggerFactory loggerFactory)
            {
                #region Contracts

                if (logger == null) throw new ArgumentException(nameof(logger));
                if (loggerFactory == null) throw new ArgumentException(nameof(loggerFactory));

                #endregion

                // Default
                _logger = logger;
                _loggerFactory = loggerFactory;
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
                    _logger.LogWarning("Information Message");
                    _logger.LogError("Error Message");
                    _logger.LogCritical("Critical Message");
                    Console.WriteLine("\n");

                    // Logger-Exception
                    Console.WriteLine("=====Logger-Exception=====");
                    _logger.LogTrace(this.GetException("Trace Exception"), "Trace Message");
                    _logger.LogDebug(this.GetException("Debug Exception"), "Debug Message");
                    _logger.LogInformation(this.GetException("Information Exception"), "Information Message");
                    _logger.LogWarning(this.GetException("Information Exception"), "Information Message");
                    _logger.LogError(this.GetException("Error Exception"), "Error Message");
                    _logger.LogCritical(this.GetException("Critical Exception"), "Critical Message");
                    Console.WriteLine("\n");

                    // LoggerFactory
                    Console.WriteLine("=====LoggerFactory-Message=====");
                    var logger = _loggerFactory.CreateLogger<ProgramService>();
                    logger.LogTrace("{0} Message\n", "Trace");
                    logger.LogDebug("{0} Message\n", "Debug");
                    logger.LogInformation("{0} Message\n", "Information");
                    logger.LogWarning("{0} Message\n", "Warning");
                    logger.LogError("{0} Message\n", "Error");
                    logger.LogCritical("{0} Message\n", "Critical");
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