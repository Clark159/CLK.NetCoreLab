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
using System.Net.Http;
using System.Text;
using System.Net;
using System.IO;

namespace TraceContextLab.SubService
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
                    // HttpClient
                    services.AddHttpClient();

                    // OpenTelemetry
                    services.AddOpenTelemetryTracing((builder) =>
                    {
                        // Resource
                        builder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("CLK-TraceContextLab-SubService"));

                        // Source
                        builder.AddSource("CLK.TraceContextLab.SubModule");

                        // Exporter
                        builder.AddConsoleExporter();
                        builder.AddJaegerExporter(options =>
                        {
                            options.AgentHost = "localhost";
                        });
                    });

                    // ConsoleService
                    services.AddHostedService<ConsoleService>();
                })
            ;
        }


        // Class
        public class ConsoleService : BackgroundService
        {
            // Fields
            private static ActivitySource _activitySource = new ActivitySource("CLK.TraceContextLab.SubModule");


            // Methods
            protected override Task ExecuteAsync(CancellationToken stoppingToken)
            {
                return Task.Run(() => {

                    // Variables
                    HttpListener httpListener = null;
                    HttpListenerContext httpContext = null;

                    // Execute
                    try
                    {
                        // HttpListener
                        httpListener = new System.Net.HttpListener();
                        httpListener.Prefixes.Add(@"http://localhost:8080/");
                        httpListener.Start();

                        // Listen
                        while (true)
                        {
                            httpContext = httpListener.GetContext();
                            if (httpContext != null) break;
                        }

                        // ParentContext
                        ActivityContext parentContext = default;
                        var traceParent = httpContext.Request.Headers["traceparent"];
                        var traceState = httpContext.Request.Headers["tracestate"];
                        if (string.IsNullOrEmpty(traceParent) == false) parentContext = ActivityContext.Parse(traceParent, traceState);

                        // Calculate
                        using (var calculateActivity = _activitySource.StartActivity("Calculate", ActivityKind.Server, parentContext))
                        {
                            // Execute
                            Thread.Sleep(1000);
                            calculateActivity?.SetTag("Amount", "500");
                        }

                        // Response
                        httpContext.Response.StatusCode = 200;
                        using (var streamWriter = new StreamWriter(httpContext.Response.OutputStream))
                        {
                            streamWriter.Write(@"{amount:500}");
                            streamWriter.Close();
                        }
                        httpContext.Response.Close();
                    }
                    finally
                    {
                        // Dispose
                        Thread.Sleep(1000);
                        httpListener?.Stop();
                        httpListener?.Close();
                    }
                });
            }
        }
    }
}
