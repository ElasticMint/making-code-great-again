using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace MakingCodeGreatAgain.After.Logging
{
    internal static class LoggingLevel
    {
        public static LoggingLevelSwitch MinimumLoggingLevel { get; } = new LoggingLevelSwitch();

        public static void Update(LogEventLevel logEventLevel)
        {
            Log.Information(
                "Updating logging level from {@previousLoggingLevel} to {@newLoggingLevel}",
                MinimumLoggingLevel.MinimumLevel,
                logEventLevel);
            MinimumLoggingLevel.MinimumLevel = logEventLevel;
        }
    }
}