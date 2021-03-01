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

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
                .CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<Autofac.ContainerBuilder>((containerBuilder) =>
                {
                    containerBuilder
                        .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                        .As<IHostedService>();
                })
            ;
        }
    }

    public class ConsoleService : IHostedService
    {
        // Fields
        private readonly ILogger<ConsoleService> _logger = null;


        // Constructors
        public ConsoleService(ILogger<ConsoleService> logger)
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
            Console.WriteLine("ConsoleService:StartAsync");

            // Return
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Notify
            Console.WriteLine("ConsoleService:StopAsync");

            // Return
            return Task.CompletedTask;
        }
    }
}
