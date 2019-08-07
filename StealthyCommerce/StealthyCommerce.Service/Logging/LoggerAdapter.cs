using Microsoft.Extensions.Logging;

namespace StealthyCommerce.Service.Logging
{
    public class LoggerAdapter<T> : ILoggerAdapter<T>
    {
        private ILogger<T> logger;

        public LoggerAdapter(ILogger<T> logger)
        {
            this.logger = logger;
        }

        public void LogError(string message, params object[] args)
        {
            this.logger.LogError(message, args);
        }

        public void LogInformation(string message, params object[] args)
        {
            this.logger.LogInformation(message, args);
        }
    }
}
