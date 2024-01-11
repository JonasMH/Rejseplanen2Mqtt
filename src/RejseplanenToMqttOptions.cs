// See https://aka.ms/new-console-template for more information
namespace Rejseplanen2Mqtt.Client;

public class RejseplanenToMqttOptions
{
    public List<TripToInform> TripsToPublish { get; set; } = [
        new TripToInform {
            Name = "Aarhus to Hedensted",
            OriginId = "8600053", // Aarhus H
			DestId = "8600071", // Hedensted St.
            Time = "13:30"
		},
        new TripToInform {
            Name = "Hedensted to Aarhus",
            OriginId = "8600071", // Hedensted St.
			DestId  = "8600053", // Aarhus H
            Time = "6:50"
        },
    ];
}
