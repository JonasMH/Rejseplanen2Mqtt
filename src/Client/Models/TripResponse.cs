using System.Text.Json.Serialization;

namespace Rejseplanen2Mqtt.Client;

public class ApiTripResponse
{
    [JsonPropertyName("TripList")]
    public TripList TripList { get; set; } = null!;
}
