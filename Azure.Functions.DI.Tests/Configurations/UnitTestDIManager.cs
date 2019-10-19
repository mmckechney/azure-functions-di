using Azure.Functions.DI.Interfaces;
using Azure.Functions.DI.Managers;
using Microsoft.Extensions.DependencyInjection;

namespace Azure.Functions.DI.Tests.Configurations
{
    public class UnitTestDIManager
    {
        public static ServiceProvider ConfigureServices()
        {
            var configManager = new ConfigManager();
            
            var services = new ServiceCollection();
            services.AddSingleton<ISimpleManager, SimpleManager>();
            services.AddSingleton(configManager);

            return services.BuildServiceProvider();
        }
    }
}