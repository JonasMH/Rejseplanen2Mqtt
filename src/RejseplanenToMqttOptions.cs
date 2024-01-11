// See https://aka.ms/new-console-template for more information
namespace Rejseplanen2Mqtt.Client;

public class RejseplanenToMqttOptions
{
    /// <summary>
    /// Update interval in seconds
    /// </summary>
    public long UpdateIntervalSeconds { get; set; } = 300; // Every 5 mins
    public List<TripToPublish> TripsToPublish { get; set; } = [];
}
