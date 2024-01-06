using System.Text.Json.Serialization;

namespace Rejseplanen2Mqtt.Client
{
	public class DeparturBoardResponse
    {
        [JsonPropertyName("DepartureBoard")]
        public DepartureBoard DepartureBoard { get; set; }
    }
}
