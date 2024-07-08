using Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Extensions;

public static class AddAmazonSecretsManagerExtension
{
    public static void AddAmazonSecretsManager(this IConfigurationBuilder configurationBuilder, 
        string region,
        string secretName)
    {
        var configurationSource = 
            new AmazonSecretsManagerConfigurationSource(region, secretName);

        configurationBuilder.Add(configurationSource);
    }
}