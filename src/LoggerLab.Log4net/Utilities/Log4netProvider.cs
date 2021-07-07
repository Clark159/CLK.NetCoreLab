using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerLab.Log4net
{
    public class Log4netLoggerProvider : ILoggerProvider
    {
        // Fields
        private readonly ILoggerRepository _loggerRepository = null;


        // Constructors
        public Log4netLoggerProvider(string configFileName)
        {
            #region Contracts

            if (string.IsNullOrEmpty(configFileName) == true) throw new ArgumentException(nameof(configFileName));

            #endregion

            // LoggerRepository
            _loggerRepository = this.CreateLoggerRepository(configFileName);
            if (_loggerRepository == null) throw new InvalidOperationException($"{nameof(_loggerRepository)}=null");
        }

        public void Dispose()
        {
            // Shutdown
            _loggerRepository.Shutdown();
        }


        // Methods
        public ILogger CreateLogger(string categoryName)
        {
            #region Contracts

            if (string.IsNullOrEmpty(categoryName) == true) throw new ArgumentException(nameof(categoryName));

            #endregion

            // Create
            return new Log4netLogger(categoryName);
        }

        private ILoggerRepository CreateLoggerRepository(string configFileName)
        {
            #region Contracts

            if (string.IsNullOrEmpty(configFileName) == true) throw new ArgumentException(nameof(configFileName));

            #endregion

            // LoggerRepository
            var loggerRepository = LogManager.GetRepository();
            if (loggerRepository == null) throw new InvalidOperationException($"{nameof(loggerRepository)}=null");

            // Configure
            XmlConfigurator.Configure(loggerRepository, new FileInfo(configFileName));
                                   
            // Return
            return loggerRepository;
        }
    }
}
