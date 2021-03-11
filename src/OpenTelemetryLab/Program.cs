using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using OpenTelemetry;
using OpenTelemetry.Trace;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;

namespace OpenTelemetryLab
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
                .ConfigureServices((services)=>
                {
                    // OpenTelemetry
                    services.AddOpenTelemetryTracing((tracerProvider) =>
                    {
                        // Resource
                        tracerProvider.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("CLK-OpenTelemetryLab-Service"));
                        
                        // Source
                        tracerProvider.AddSource("CLK.OpenTelemetryLab.MainModule");

                        // Exporter
                        tracerProvider.AddConsoleExporter();
                        tracerProvider.AddJaegerExporter(options =>
                        {
                            options.AgentHost = "localhost";
                        });
                    });

                    // ConsoleService
                    services.AddHostedService<ConsoleService>();
                });


        // Class
        public class ConsoleService : BackgroundService
        {
            // Fields
            private static ActivitySource _activitySource = new ActivitySource("CLK.OpenTelemetryLab.MainModule");


            // Methods
            protected override Task ExecuteAsync(CancellationToken stoppingToken)
            {
                return Task.Run(() => {

                    // Pay
                    using (var payActivity = _activitySource.StartActivity("Pay", ActivityKind.Internal))
                    {
                        // Execute
                        Thread.Sleep(1000);
                        payActivity?.SetTag("User", "Clark");

                        // Print
                        using (var printActivity = _activitySource.StartActivity("Print", ActivityKind.Internal))
                        {
                            // Execute
                            Thread.Sleep(1000);
                            printActivity?.SetTag("User", "Jane");
                        }

                        // Sleep
                        Thread.Sleep(1000);
                    }

                });
            }
        }
    }
}
