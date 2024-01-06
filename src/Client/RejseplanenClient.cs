using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Rejseplanen2Mqtt.Client
{
	public class RejseplanenClientOptions
    {
        public string BaseUrl { get; set; } = "https://xmlopen.rejseplanen.dk/";
    }

    public class RejseplanenClient
    {
        private readonly HttpClient _httpClient;

        public RejseplanenClient(HttpClient httpClient, RejseplanenClientOptions options)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(options.BaseUrl);
        }

        public async Task<DeparturBoardResponse> DepartureBoardAsync(DeparturBoardRequestOptions options)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri($"/bin/rest.exe/departureBoard?id={options.StopId}&format=json", UriKind.Relative)
            };
            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return (await response.Content.ReadFromJsonAsync<DeparturBoardResponse>())!;
        }

        public async Task<List<List<TripLeg>>> TripAsync(TripRequestOptions options)
		{
			var request = new HttpRequestMessage()
			{
				RequestUri = new Uri($"/bin/rest.exe/trip?originId={options.OriginId}&destId={options.DestId}&format=json", UriKind.Relative)
			};
			var response = await _httpClient.SendAsync(request);

			response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<TripResponse>();
            var result = new List<List<TripLeg>>();


            foreach (var trip in json.TripList.Trips)
            {
                if(trip.Leg.GetValueKind() == JsonValueKind.Array)
                {
                    result.Add(JsonSerializer.Deserialize<List<TripLeg>>(trip.Leg.AsArray())!);
                } else if (trip.Leg.GetValueKind() == JsonValueKind.Object)
				{
					result.Add([JsonSerializer.Deserialize<TripLeg>(trip.Leg.AsObject())!]);
				} else
                {
                    throw new InvalidOperationException();
                }
			}

			return result;
		}
    }

    public class TripRequestOptions
    {
        public string OriginId { get; set; }
        public string DestId { get; set; }
    }
    public class TripResponse
    {
        public TripList TripList { get; set; }
    }

    public class TripList
    {
        [JsonPropertyName("Trip")]
        public List<Trip> Trips { get; set; }
	}

    public class Trip
    {
        [JsonPropertyName("Leg")]
        public JsonNode Leg { get; set; }
    }


    public class TripLeg
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("Origin")]
		public TripLegStation Origin { get; set; }

		[JsonPropertyName("Destination")]
		public TripLegStation Destination { get; set; }
    }
    public class TripLegStation
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
		[JsonPropertyName("type")]
		public string Type { get; set; }
		[JsonPropertyName("routeIdx")]
		public string RouteIdx { get; set; }
		[JsonPropertyName("time")]
		public string Time { get; set; }
		[JsonPropertyName("date")]
		public string Date { get; set; }
		[JsonPropertyName("track")]
		public string Track { get; set; }
		[JsonPropertyName("rtTrack")]
		public string RtTrack { get; set; }
    }
}
