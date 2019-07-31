using Core.Application;
using Infrastructure;
using Microsoft.AspNetCore.Hosting;

namespace DependencyInjection
{
    public static class ServicesRegistrar
    {
        public static IWebHostBuilder AddToWebHost(this IWebHostBuilder webHostBuilder, ISettings settings)
        {
            LoggerStartup.ConfigureLog(settings);
            return webHostBuilder.UseCountyHomeLogger();
        }
        public static void RegisterServices(ISettings settings, IDependencyProvider dependencyProvider)
        {
        }
    }
}
