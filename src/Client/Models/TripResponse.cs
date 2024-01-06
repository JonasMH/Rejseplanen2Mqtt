using System.Text.Json.Serialization;

namespace Rejseplanen2Mqtt.Client;

public class TripResponse
{
    [JsonPropertyName("TripList")]
    public TripList TripList { get; set; } = null!;
}
