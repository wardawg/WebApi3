using System;

namespace ReposServices.Logging
{
    public static class LoggingExtensions
    {
        public static void Debug(this ILogger logger, string message, Exception exception = null, ServiceInfo serviceInfo = null)
        {
            FilteredLog(logger, LogLevel.Debug, message, exception, serviceInfo);
        }
        public static void Information(this ILogger logger, string message, Exception exception = null, ServiceInfo serviceInfo = null)
        {
            FilteredLog(logger, LogLevel.Information, message, exception, serviceInfo);
        }
        public static void Warning(this ILogger logger, string message, Exception exception = null, ServiceInfo serviceInfo = null)
        {
            FilteredLog(logger, LogLevel.Warning, message, exception, serviceInfo);
        }
        public static void Error(this ILogger logger, string message, Exception exception = null, ServiceInfo serviceInfo = null)
        {
            FilteredLog(logger, LogLevel.Error, message, exception, serviceInfo);
        }
        public static void Fatal(this ILogger logger, string message, Exception exception = null, ServiceInfo serviceInfo = null)
        {
            FilteredLog(logger, LogLevel.Fatal, message, exception, serviceInfo);
        }

        private static void FilteredLog(ILogger logger, LogLevel level, string message, Exception exception = null, ServiceInfo serviceInfo = null)
        {
            //don't log thread abort exception
            if (exception is System.Threading.ThreadAbortException)
                return;

            if (logger.IsEnabled(level))
            {
                string fullMessage = exception == null ? string.Empty : exception.ToString();
                logger.InsertLog(level, message, fullMessage, serviceInfo);
            }
        }
    }
}
