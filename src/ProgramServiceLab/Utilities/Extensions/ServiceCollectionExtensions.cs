using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProgramServiceLab
{
    public static class ServiceCollectionExtensions
    {
        // Methods
        public static IServiceCollection AddProgramService<TProgramService>(this IServiceCollection services) where TProgramService : class
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));

            #endregion

            // Add
            services.AddSingleton<IHostedService, ProgramService<TProgramService>>();

            // Return
            return services;
        }
    }
}
