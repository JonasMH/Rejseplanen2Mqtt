using System.Text.Json.Serialization;

namespace Rejseplanen2Mqtt.Client
{
	public class DepartureBoard
    {
        [JsonPropertyName("departure")]
        public List<Departure> Departures { get; set; }
    }
}
