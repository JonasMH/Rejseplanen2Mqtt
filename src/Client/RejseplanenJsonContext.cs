using System.Text.Json.Serialization;
using Rejseplanen2Mqtt.Client;

namespace Rejseplanen2Mqtt.Client;

[JsonSerializable(typeof(DepartureBoardResponse))]
[JsonSerializable(typeof(TripResponse))]
[JsonSerializable(typeof(ApiTripResponse))]
[JsonSerializable(typeof(TripLeg))]
[JsonSerializable(typeof(List<TripLeg>))]
[JsonSerializable(typeof(TripMqttStatusUpdate))]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    WriteIndented = true
    )]
public partial class RejseplanenJsonContext : JsonSerializerContext
{

}
