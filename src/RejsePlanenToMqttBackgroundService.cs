// See https://aka.ms/new-console-template for more information
using MQTTnet;
using NodaTime;
using NodaTime.Text;
using Rejseplanen2Mqtt.Client;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using ToMqttNet;

namespace Rejseplanen2Mqtt.Client;

public class RejsePlanenToMqttBackgroundService(
    ILogger<RejsePlanenToMqttBackgroundService> logger,
    RejseplanenClient rejseplanenClient,
    MqttConnectionService mqtt) : BackgroundService
{
	private readonly ILogger<RejsePlanenToMqttBackgroundService> _logger = logger;
	private readonly RejseplanenClient _rejseplanenClient = rejseplanenClient;
	private readonly MqttConnectionService _mqtt = mqtt;
	private readonly List<TripToInform> _tripsToPublish = [
		new TripToInform {
			Name = "Aarhus to Hedensted",
			OriginId = "8600053", // Aarhus H
			DestId = "8600071", // Hedensted St.
		},
		new TripToInform {
			Name = "Hedensted to Aarhus",
			OriginId = "8600071", // Hedensted St.
			DestId  = "8600053", // Aarhus H
		},
	];

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		foreach (var trip in _tripsToPublish)
		{
			var discoveryDoc = new MqttSensorDiscoveryConfig()
			{
				UniqueId = "rejseplanen_" + trip.Name.ToLower().Replace(" ", "_"),
				Name = trip.Name,
				UnitOfMeasurement = HomeAssistantUnits.TIME_MINUTES.Value,
				ValueTemplate = "{{ value_json.value }}",
				StateTopic = _mqtt.MqttOptions.NodeId + "/status/trips/" + trip.Name.ToLower().Replace(" ", "_"),
				JsonAttributesTopic = _mqtt.MqttOptions.NodeId + "rejseplanen/status/trips/" + trip.Name.ToLower().Replace(" ", "_"),
				JsonAttributesTemplate = "{{ value_json.attributes }}",
			};

			await _mqtt.PublishDiscoveryDocument(discoveryDoc);
		}

		while (!stoppingToken.IsCancellationRequested)
		{
			foreach (var tripToPublish in _tripsToPublish)
			{
				var response = await _rejseplanenClient.TripAsync(new TripRequestOptions
				{
					OriginId = tripToPublish.OriginId,
					DestId = tripToPublish.DestId
				});

				var result = new TripMqttStatusUpdate
				{
					Value = -1,
				};

				// Filter out any trips with multiple legs
				var directTrips = response.Where(x => x.Count == 1).ToList();

				var nextTrip = directTrips.FirstOrDefault()?.FirstOrDefault();

				if(nextTrip != null)
				{
					var timePattern = LocalTimePattern.CreateWithInvariantCulture("HH:mm"); // 13:30
					var datePattern = LocalDatePattern.CreateWithInvariantCulture("dd.MM.yy"); // 06.01.24

					var nextDepatureTime = timePattern.Parse(nextTrip.Origin.Time).Value;
					var nextDepatureDate = datePattern.Parse(nextTrip.Origin.Date).Value;

					var nextDepature = nextDepatureTime.On(nextDepatureDate);
					var localTime = SystemClock.Instance.GetCurrentInstant().InZone(DateTimeZoneProviders.Tzdb.GetZoneOrNull("Europe/Copenhagen")!).LocalDateTime;

					var timeToNext = Period.Between(localTime, nextDepature, PeriodUnits.Minutes);
					result.Value = timeToNext.Minutes;
				}

				await _mqtt.PublishAsync(new MqttApplicationMessageBuilder()
					.WithTopic(_mqtt.MqttOptions.NodeId + "/status/trips/" + tripToPublish.Name.ToLower().Replace(" ", "_"))
					.WithPayload(JsonSerializer.Serialize(result, RejseplanenJsonContext.Default.TripMqttStatusUpdate))
					.Build());

			}
			await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
		}
	}
}

public class TripMqttStatusUpdate
{
	[JsonPropertyName("value")]
	public long Value { get; set; }
    [JsonPropertyName("attributes")]
	public JsonNode Attributes { get; set; } = null!;
}

public class TripToInform
{
	public string Name { get; set; } = null!;
    public string OriginId { get; set; } = null!;
    public string DestId { get; set; } = null!;
}
