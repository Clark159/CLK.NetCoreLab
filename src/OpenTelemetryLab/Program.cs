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

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
                .CreateDefaultBuilder(args)
                .ConfigureServices(services=>
                {
                    // OpenTelemetry
                    services.AddOpenTelemetryTracing((builder) =>
                    {
                        // Resource
                        builder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("CLK-OpenTelemetryLab-Service"));
                        
                        // Source
                        builder.AddSource("CLK.OpenTelemetryLab.Test001");
                        builder.AddSource("CLK.OpenTelemetryLab.Test002");

                        // Exporter
                        builder.AddConsoleExporter();
                        builder.AddJaegerExporter(options =>
                        {
                            options.AgentHost = "localhost";
                        });
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
        private static ActivitySource _activitySource001 = new ActivitySource("CLK.OpenTelemetryLab.Test001");

        private static ActivitySource _activitySource002 = new ActivitySource("CLK.OpenTelemetryLab.Test002");


        // Methods
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
           return Task.Run(() =>{

               // Pay
               using (var activity001 = _activitySource002.StartActivity("Pay"))
               {
                   // Execute
                   Thread.Sleep(1000);
                   activity001?.SetTag("User", "Clark");

                   // Print
                   using (var activity002 = _activitySource002.StartActivity("Print"))
                   {
                       // Execute
                       Thread.Sleep(1000);
                       activity002?.SetTag("User", "Jane");
                   }

                   // Sleep
                   Thread.Sleep(1000);
               }

           });
        }
    }
}
