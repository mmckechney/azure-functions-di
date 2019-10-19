using Microsoft.Extensions.Configuration;

namespace Azure.Functions.DI.Managers
{
    public class ConfigManager
    {
        public readonly IConfigurationRoot _configurations;
        public ConfigManager()
        {
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            _configurations = config;
        }

        public string ApplicationInsightsInstrumentationKey => _configurations["APPINSIGHTS_INSTRUMENTATIONKEY"];
    }
}
