using System.Text.Json.Serialization;

namespace Rejseplanen2Mqtt.Client;

public class DepartureBoardResponse
{
    [JsonPropertyName("DepartureBoard")]
    public DepartureBoard DepartureBoard { get; set; } = null!;
}
