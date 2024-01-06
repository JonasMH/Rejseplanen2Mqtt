using System.Text.Json.Serialization;

namespace Rejseplanen2Mqtt.Client;

public class TripList
{
    [JsonPropertyName("Trip")]
    public List<Trip> Trips { get; set; } = null!;
}
