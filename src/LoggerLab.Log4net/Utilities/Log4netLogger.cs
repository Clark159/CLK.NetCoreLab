using log4net;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerLab.Log4net
{
    public class Log4netLogger : ILogger
    {
        // Fields
        private readonly ILog _logger = null;


        // Constructors
        internal Log4netLogger(string categoryName)
        {
            #region Contracts

            if (string.IsNullOrEmpty(categoryName) == true) throw new ArgumentException(nameof(categoryName));

            #endregion

            // Logger
            _logger = LogManager.GetLogger(categoryName);
            if (_logger == null) throw new InvalidOperationException("_logger=null");
        }


        // Methods
        public IDisposable BeginScope<TState>(TState state)
        {
            // Return
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            // Mapping
            switch (logLevel)
            {
                case LogLevel.Trace: return _logger.IsDebugEnabled;
                case LogLevel.Debug: return _logger.IsDebugEnabled;
                case LogLevel.Information: return _logger.IsInfoEnabled;
                case LogLevel.Warning: return _logger.IsWarnEnabled;
                case LogLevel.Error: return _logger.IsErrorEnabled;
                case LogLevel.Critical: return _logger.IsFatalEnabled;
                case LogLevel.None: return false;
                default: throw new InvalidOperationException($"Unknown logLevel: {nameof(logLevel)}={logLevel}");
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            // Require
            if (this.IsEnabled(logLevel) == false) return;
            if (formatter == null) throw new InvalidOperationException($"{nameof(formatter)}=null");

            // Message
            var message = formatter(state, exception);
            if (string.IsNullOrEmpty(message) == true) message = string.Empty;

            // Write
            switch (logLevel)
            {
                case LogLevel.Trace: _logger.Debug(message, exception); break;
                case LogLevel.Debug: _logger.Debug(message, exception); break;
                case LogLevel.Information: _logger.Info(message, exception); break;
                case LogLevel.Warning: _logger.Warn(message, exception); break;
                case LogLevel.Error: _logger.Error(message, exception); break;
                case LogLevel.Critical: _logger.Fatal(message, exception); break;
                case LogLevel.None: break;
                default: throw new InvalidOperationException($"Unknown logLevel: {nameof(logLevel)}={logLevel}");
            }
        }
    }
}
