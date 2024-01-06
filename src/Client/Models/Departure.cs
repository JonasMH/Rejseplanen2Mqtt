using System.Text.Json.Serialization;

namespace Rejseplanen2Mqtt.Client;

	public class Departure
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;
    [JsonPropertyName("stop")]
    public string Stop { get; set; } = null!;
    [JsonPropertyName("time")]
    public string Time { get; set; } = null!;
    [JsonPropertyName("date")]
    public string Date { get; set; } = null!;
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    [JsonPropertyName("line")]
    public string Line { get; set; } = null!;
    [JsonPropertyName("messages")]
    public string Messages { get; set; } = null!;
    [JsonPropertyName("rtTime")]
    public string RtTime { get; set; } = null!;
    [JsonPropertyName("rtDate")]
    public string RtDate { get; set; } = null!;
    [JsonPropertyName("finalStop")]
    public string FinalStop { get; set; } = null!;
    [JsonPropertyName("direction")]
    public string Direction { get; set; } = null!;
    public JourneyDetailRef JourneyDetailRef { get; set; } = null!;
}
