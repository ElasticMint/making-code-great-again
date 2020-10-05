using AWS.Logger.SeriLog;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace MakingCodeGreatAgain.After.Logging
{
    internal static class LoggerFactory
    {
        public static ILogger Create(IConfiguration configuration, string environment)
        {
            var loggingConfiguration = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .MinimumLevel.ControlledBy(LoggingLevel.MinimumLoggingLevel)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning);

            if (environment?.ToLowerInvariant() == "development")
            {
                ConfigureForDevelopment(loggingConfiguration);
            }
            else
            {
                ConfigureForDeployedEnvironment(loggingConfiguration, configuration);
            }

            return loggingConfiguration.CreateLogger();
        }

        private static void ConfigureForDevelopment(LoggerConfiguration loggingConfiguration)
        {
            loggingConfiguration.WriteTo.Async(x => x.Console());
            loggingConfiguration.WriteTo.Async(x => x.RollingFile(new RenderedCompactJsonFormatter(), ".\\Logs\\ApiLogs-{Date}.log"));
        }

        private static void ConfigureForDeployedEnvironment(LoggerConfiguration loggingConfiguration, IConfiguration configuration)
        {
            LoggingLevel.Update(LogEventLevel.Error);
            loggingConfiguration.WriteTo.AWSSeriLog(configuration, textFormatter: new RenderedCompactJsonFormatter());
        }
    }
}
