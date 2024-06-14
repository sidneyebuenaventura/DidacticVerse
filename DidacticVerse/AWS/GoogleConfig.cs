using Newtonsoft.Json;

namespace DidacticVerse.AWS;

public class GoogleConfig
{
    [JsonProperty("client_id")]
    public string ClientId { get; set; }

    [JsonProperty("client_secret")]
    public string ClientSecret { get; set; }
}
