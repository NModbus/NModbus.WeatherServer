using Microsoft.Extensions.Logging;
using NModbus;

namespace ModbusWeatherServer
{
    public class ModbusLogger : IModbusLogger
    {
        private readonly ILogger _logger;

        public ModbusLogger(ILogger<ModbusLogger> logger)
        {
            _logger = logger;
        }

        public void Log(LoggingLevel level, string message)
        {
            _logger.LogInformation("[{Level}] {Message}", level, message);
        }

        public bool ShouldLog(LoggingLevel level)
        {
            return true;
        }
    }
}
