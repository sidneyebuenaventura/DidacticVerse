using Amazon.Runtime.CredentialManagement;
using Amazon.Runtime;
using Amazon.SecretsManager.Extensions.Caching;
using Amazon.SecretsManager;
using Newtonsoft.Json;

namespace DidacticVerse.AWS;

public class Secrets
{
    private readonly IConfiguration _configuration;
    private readonly SecretsManagerCache _cache;

    public Secrets(IConfiguration configuration)
    {
        _configuration = configuration;

        var chain = new CredentialProfileStoreChain();
        AWSCredentials basicProfile;
        if (!chain.TryGetAWSCredentials(_configuration["AWS:Profile"], out basicProfile))
        {
            throw new Exception($"Could not get AWS profile credentials for {_configuration["AWS:Profile"]}");
        }
        var secretsManagerClient = new AmazonSecretsManagerClient(basicProfile, _configuration.GetAWSOptions().Region);
        _cache = new SecretsManagerCache(secretsManagerClient);

    }

    public T? Get<T>(string name)
        where T : class
    {

        var result = _cache.GetSecretString(name).Result;
        var secret = JsonConvert.DeserializeObject<T>(result);

        return secret;
    }
}
