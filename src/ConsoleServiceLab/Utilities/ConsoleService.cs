using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleServiceLab
{
    public class ConsoleService<TConsoleService> : BackgroundService where TConsoleService : class
    {
        // Fields
        private readonly IServiceProvider _serviceProvider = null;

        private readonly IHostApplicationLifetime _hostApplicationLifetime = null;


        // Constructors
        public ConsoleService(IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime)
        {
            #region Contracts

            if (serviceProvider == null) throw new ArgumentException(nameof(serviceProvider));
            if (hostApplicationLifetime == null) throw new ArgumentException(nameof(hostApplicationLifetime));

            #endregion

            // Default
            _serviceProvider = serviceProvider;
            _hostApplicationLifetime = hostApplicationLifetime;
        }


        // Methods
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Execute
            return Task.Run(() =>
            {
                // Run
                ServiceActivator.ExecuteMethod<TConsoleService>(_serviceProvider, "Run");

                // End
                _hostApplicationLifetime.StopApplication();
            });
        }
    }
}
