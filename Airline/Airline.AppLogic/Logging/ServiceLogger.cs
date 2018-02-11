using System;
using NLog;

namespace Airline.AppLogic.Logging
{
    /// <summary>
    /// Nlog ILogger wrapper.
    /// </summary>
    public class ServiceLogger : IServiceLogger
    {
        public ServiceLogger(string name)
        {
            _logger = LogManager.GetLogger(name);
        }

        public void Debug(string message)
        {
            Log(LogLevel.Debug, message);
        }

        public void Info(string message)
        {
            Log(LogLevel.Info, message);
        }

        public void Warning(string message)
        {
            Log(LogLevel.Warn, message);
        }

        public void Error(string message)
        {
            Log(LogLevel.Error, message);
        }

        private void Log(LogLevel lvl, string message)
        {
            var logEvent = new LogEventInfo(lvl, _logger.Name, message);

            logEvent.Properties["EventID"] = new Guid().ToString();

            _logger.Log(typeof(ServiceLogger), logEvent);
        }

        public void Dispose()
        {}

        private ILogger _logger;
    }
}
