using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Rejseplanen2Mqtt.Client;

public class Trip
{
    [JsonPropertyName("cancelled")]
    public string? Cancelled { get; set; }
    [JsonPropertyName("Leg")]
    public JsonNode Leg { get; set; } = null!;
}

public class TripResponse
{
    public bool Cancelled { get; set; }
    public List<TripLeg> Legs { get; set; } = new List<TripLeg>();
}
