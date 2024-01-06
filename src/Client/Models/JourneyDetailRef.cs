using System.Text.Json.Serialization;

namespace Rejseplanen2Mqtt.Client;

	public class JourneyDetailRef
{
    [JsonPropertyName("ref")]
    public string Ref { get; set; }
}
