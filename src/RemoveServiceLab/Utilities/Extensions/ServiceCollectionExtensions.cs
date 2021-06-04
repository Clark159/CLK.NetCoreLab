using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RemoveServiceLab
{
    public static class ServiceCollectionExtensions
    {
        // Methods
        public static IServiceCollection RemoveService<TService>(this IServiceCollection services) where TService : class
        {
            #region Contracts

            if (services == null) throw new ArgumentException(nameof(services));

            #endregion

            // ServiceDescriptor
            var serviceDescriptor = services.FirstOrDefault(o => o.ImplementationType == typeof(TService));
            if (serviceDescriptor == null) return services;

            // Remove
            services.Remove(serviceDescriptor);

            // Return
            return services;
        }
    }
}
