using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Azure.Functions.DI.Managers;
using Azure.Functions.DI;
using Azure.Functions.DI.Interfaces;

[assembly: FunctionsStartup(typeof(StartupJob))]
namespace Azure.Functions.DI
{
    public class StartupJob : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configManager = new ConfigManager();
            
            // Configure services
            var services = builder.Services;
            services.AddSingleton<ISimpleManager, SimpleManager>();
            services.AddSingleton(configManager);
            
            services.BuildServiceProvider();
        }
    }
}
