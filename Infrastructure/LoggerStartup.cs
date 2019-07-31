using Core.Application;
using Microsoft.AspNetCore.Hosting;
using NLog.Targets;
using NLog.Web;
using System.IO;
using System.Linq;

namespace Infrastructure
{
    public static class LoggerStartup
    {
        public static void ConfigureLog(ISettings settings)
        {
            var config = new NLog.Config.LoggingConfiguration();
            var logConfig = settings.LogConfig;

            var target = new FileTarget("Web.UI")
            {
                FileName = logConfig.LogDirectory + "/Web.Ui.log",
                Layout = new NLog.Layouts.SimpleLayout(settings.LogConfig.Layout)
            };

            config.AddTarget(target);
            if (!string.IsNullOrEmpty(logConfig.LogDirectory) && !Directory.Exists(logConfig.LogDirectory))
            {
                Directory.CreateDirectory(logConfig.LogDirectory);
            }
            config.AddRule(LogLevel(settings.LogConfig.MinLogLevel), NLog.LogLevel.Fatal, target, "County_Home.*");

            NLogBuilder.ConfigureNLog(config);
        }

        public static IWebHostBuilder UseCountyHomeLogger(this IWebHostBuilder webHostBuilder)
        {
            return webHostBuilder.UseNLog();
        }

        private static NLog.LogLevel LogLevel(string logLevel)
        {
            return NLog.LogLevel.AllLevels
                .SingleOrDefault(x => string.Equals(x.Name, logLevel, System.StringComparison.OrdinalIgnoreCase));
        }
    }
}
