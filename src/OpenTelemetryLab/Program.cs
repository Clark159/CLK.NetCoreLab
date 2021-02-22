using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using OpenTelemetry;
using OpenTelemetry.Trace;

namespace OpenTelemetryLab
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
                .ConfigureServices(services=>
                {
                    // OpenTelemetry
                    services.AddOpenTelemetryTracing((builder) =>
                    {
                        // Source
                        builder.AddSource("CLK.ActivitySourceLab.Test001");
                        builder.AddSource("CLK.ActivitySourceLab.Test002");

                        // Exporter
                        builder.AddConsoleExporter();
                    });

                    // ClarkApp
                    services.AddHostedService<ClarkApp>();
                })
            ;
        }
    }

    public class ClarkApp : BackgroundService
    {
        // Fields
        private static ActivitySource _activitySource001 = new ActivitySource("CLK.ActivitySourceLab.Test001");

        private static ActivitySource _activitySource002 = new ActivitySource("CLK.ActivitySourceLab.Test002");


        // Methods
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Diagnostic
            {
                // Pay
                using (var activity001 = _activitySource002.StartActivity("Pay"))
                {
                    // Pay.SetTag
                    activity001?.SetTag("User", "Clark");

                    // Print
                    using (var activity002 = _activitySource002.StartActivity("Print"))
                    {
                        // Print.SetTag
                        activity002?.SetTag("User", "Jane");
                    }
                }
            }

            // Return
            return Task.CompletedTask;
        }
    }
}
