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

namespace TraceContextLab
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
                        builder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("CLK-TraceContextLab-MainService"));

                        // Source
                        builder.AddSource("CLK.TraceContextLab.MainModule");

                        // Instrumentation
                        //builder.AddHttpClientInstrumentation();

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
    }

    public class ConsoleService : BackgroundService
    {
        // Fields
        private static ActivitySource _activitySource = new ActivitySource("CLK.TraceContextLab.MainModule");

        private readonly IHttpClientFactory _httpClientFactory = null;


        // Constructors
        public ConsoleService(IHttpClientFactory httpClientFactory)
        {
            #region Contracts

            if (httpClientFactory == null) throw new ArgumentException(nameof(httpClientFactory));

            #endregion

            // Default
            _httpClientFactory = httpClientFactory;
        }


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
                    using (var printActivity = _activitySource.StartActivity("Print", ActivityKind.Client))
                    {
                        // Execute
                        this.CallSubService("http://localhost:8080/");
                        printActivity?.SetTag("User", "Jane");
                    }

                    // Sleep
                    Thread.Sleep(1000);
                }
            });
        }

        private void CallSubService(string requestUri)
        {
            #region Contracts

            if (string.IsNullOrEmpty(requestUri) == true) throw new ArgumentException(nameof(requestUri));

            #endregion

            // SubService
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                // RequestContent
                var requestContent = new StringContent("{user:\"Clark\"}", Encoding.UTF8, "application/json");
                if (requestContent == null) throw new InvalidOperationException($"{nameof(requestContent)}==null");

                // Post
                var response = httpClient.PostAsync(requestUri, requestContent).GetAwaiter().GetResult();
                if (response == null) throw new InvalidOperationException($"{nameof(response)}==null");
                if (response.IsSuccessStatusCode == false)
                {
                    var errorMessage = response.Content?.ReadAsStringAsync()?.GetAwaiter().GetResult()?.Trim();
                    if (string.IsNullOrEmpty(errorMessage) == false) throw new InvalidOperationException($"StatusCode={response.StatusCode}, ErrorMessage={errorMessage}");
                }
                if (response.IsSuccessStatusCode == false) throw new InvalidOperationException($"StatusCode={response.StatusCode}");

                // ResponseContent
                var responseContent = response.Content;
                if (responseContent == null) throw new InvalidOperationException($"{nameof(responseContent)}==null");
                var responseContentString = responseContent.ReadAsStringAsync().GetAwaiter().GetResult();
                if (string.IsNullOrEmpty(responseContentString) == true) throw new InvalidOperationException($"{nameof(responseContentString)}==null");
            }
        }
    }
}
