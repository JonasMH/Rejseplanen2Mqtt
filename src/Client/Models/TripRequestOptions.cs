using NodaTime;

namespace Rejseplanen2Mqtt.Client;

public class TripRequestOptions
{
    public string OriginId { get; set; } = null!;
    public string DestId { get; set; } = null!;
    public LocalDate? Date { get; set; }
    public LocalTime? Time { get; set; }
}
