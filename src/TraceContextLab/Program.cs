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
                        builder.AddSource("CLK.TraceContextLab.Test001");
                        builder.AddSource("CLK.TraceContextLab.Test002");

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
        private static ActivitySource _activitySource001 = new ActivitySource("CLK.TraceContextLab.Test001");

        private static ActivitySource _activitySource002 = new ActivitySource("CLK.TraceContextLab.Test002");

        private readonly IHttpClientFactory _httpClientFactory = null;


        // Constructors
        public ClarkApp(IHttpClientFactory httpClientFactory)
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
                using (var activity001 = _activitySource002.StartActivity("Pay"))
                {
                    // Execute
                    Thread.Sleep(1000);
                    activity001?.SetTag("User", "Clark");

                    // Print
                    using (var activity002 = _activitySource002.StartActivity("Print"))
                    {
                        // Execute
                        this.CallRemoteService("http://localhost:16686/");
                        activity002?.SetTag("User", "Jane");                        
                    }

                    // Sleep
                    Thread.Sleep(1000);
                }
            });
        }

        private void CallRemoteService(string requestUri)
        {
            #region Contracts

            if (string.IsNullOrEmpty(requestUri) == true) throw new ArgumentException(nameof(requestUri));

            #endregion

            // RemoteService
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
