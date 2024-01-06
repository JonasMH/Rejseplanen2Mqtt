using System.Text.Json.Serialization;

namespace Rejseplanen2Mqtt.Client;

	public class Departure
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("type")]
    public string Type { get; set; }
    [JsonPropertyName("stop")]
    public string Stop { get; set; }
    [JsonPropertyName("time")]
    public string Time { get; set; }
    [JsonPropertyName("date")]
    public string Date { get; set; }
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("line")]
    public string Line { get; set; }
    [JsonPropertyName("messages")]
    public string Messages { get; set; }
    [JsonPropertyName("rtTime")]
    public string RtTime { get; set; }
    [JsonPropertyName("rtDate")]
    public string RtDate { get; set; }
    [JsonPropertyName("finalStop")]
    public string FinalStop { get; set; }
    [JsonPropertyName("direction")]
    public string Direction { get; set; }
    public JourneyDetailRef JourneyDetailRef { get; set; }
}
