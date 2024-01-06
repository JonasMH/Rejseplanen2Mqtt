using System.Text.Json.Serialization;

namespace Rejseplanen2Mqtt.Client;

public class TripLeg
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    [JsonPropertyName("Origin")]
    public TripLegStation Origin { get; set; } = null!;

    [JsonPropertyName("Destination")]
    public TripLegStation Destination { get; set; } = null!;
}
