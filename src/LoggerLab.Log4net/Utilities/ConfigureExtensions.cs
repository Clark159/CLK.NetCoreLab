using log4net;
using log4net.Config;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerLab.Log4net
{
    public static class ConfigureExtensions
    {
        public static ILoggingBuilder AddLog4net(this ILoggingBuilder builder, string configFileName = "log4net.config")
        {
            #region Contracts

            if (builder == null) throw new ArgumentException(nameof(builder));
            if (string.IsNullOrEmpty(configFileName) == true) throw new ArgumentException(nameof(configFileName));

            #endregion

            // Log4netProvider
            builder.AddProvider(new Log4netLoggerProvider(configFileName));

            // Return
            return builder;
        }
    }
}
