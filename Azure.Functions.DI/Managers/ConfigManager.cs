using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

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

            // Load Key Vault values, if running in Portal
            if (config["AZURE_FUNCTIONS_ENVIRONMENT"] != "Development")
            {
                var azureKeyVaultName = config.GetConnectionStringOrSetting("KeyVaultName");
                var azureKeyVaultUrl = $"https://{azureKeyVaultName}.vault.azure.net/";

                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

                config = new ConfigurationBuilder()
                    .AddEnvironmentVariables()
                    .AddAzureKeyVault(azureKeyVaultUrl, keyVaultClient, new DefaultKeyVaultSecretManager())
                    .Build();
            }

            _configurations = config;
        }

        public string ApplicationInsightsInstrumentationKey => _configurations["APPINSIGHTS_INSTRUMENTATIONKEY"];
        public string DummySecret => _configurations["DummySecret"];
    }
}
