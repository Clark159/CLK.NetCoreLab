using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationLab
{
    public static class ConfigurationExtensions
    {
        // Methods
        public static T Bind<T>(this IConfiguration configuration, string key) where T : class, new()
        {
            #region Contracts

            if (configuration == null) throw new ArgumentException(nameof(configuration));
            if (string.IsNullOrEmpty(key) == true) throw new ArgumentException(nameof(key));

            #endregion

            // Create
            var value = new T();

            // Bind
            configuration.Bind(key, value);
                       
            // Return
            return value;
        }
    }
}
