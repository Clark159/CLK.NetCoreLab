using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace HostedServiceLab
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
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<Autofac.ContainerBuilder>((container) =>
                {
                    // HostedService
                    container.RegisterAssemblyTypes(Assembly.GetEntryAssembly()).As<IHostedService>();
                });


        // Class
        public class ProgramService : IHostedService
        {
            // Fields
            private readonly ILogger<ProgramService> _logger = null;


            // Constructors
            public ProgramService(ILogger<ProgramService> logger)
            {
                #region Contracts

                if (logger == null) throw new ArgumentException();

                #endregion

                // Default
                _logger = logger;
            }


            // Methods
            public Task StartAsync(CancellationToken cancellationToken)
            {
                // Notify
                Console.WriteLine("ProgramService:StartAsync");

                // Return
                return Task.CompletedTask;
            }

            public Task StopAsync(CancellationToken cancellationToken)
            {
                // Notify
                Console.WriteLine("ProgramService:StopAsync");

                // Return
                return Task.CompletedTask;
            }
        }
    }
}
