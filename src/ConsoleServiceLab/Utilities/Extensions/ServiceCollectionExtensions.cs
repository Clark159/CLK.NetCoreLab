using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleServiceLab
{
    public static class ServiceCollectionExtensions
    {
        // Methods
        public static IServiceCollection AddConsoleService<TConsoleService>(this IServiceCollection services) where TConsoleService : class
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));

            #endregion

            // Add
            services.AddSingleton<IHostedService, ConsoleService<TConsoleService>>();

            // Return
            return services;
        }
    }
}
