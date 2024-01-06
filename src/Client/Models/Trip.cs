using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Rejseplanen2Mqtt.Client;

public class Trip
{
    [JsonPropertyName("Leg")]
    public JsonNode Leg { get; set; } = null!;
}
