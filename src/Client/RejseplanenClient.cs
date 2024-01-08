using System.Text.Json;

namespace Rejseplanen2Mqtt.Client;

public class RejseplanenClient
{
    private readonly HttpClient _httpClient;

    public RejseplanenClient(HttpClient httpClient, RejseplanenClientOptions options)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(options.BaseUrl);
    }

    public async Task<DepartureBoardResponse> DepartureBoardAsync(DeparturBoardRequestOptions options)
    {
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri($"/bin/rest.exe/departureBoard?id={options.StopId}&format=json", UriKind.Relative)
        };
        var response = await _httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync(RejseplanenJsonContext.Default.DepartureBoardResponse))!;
    }

    public async Task<List<TripResponse>> TripAsync(TripRequestOptions options)
	{
		var request = new HttpRequestMessage()
		{
			RequestUri = new Uri($"/bin/rest.exe/trip?originId={options.OriginId}&destId={options.DestId}&format=json", UriKind.Relative)
		};
		var response = await _httpClient.SendAsync(request);

		response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync(RejseplanenJsonContext.Default.ApiTripResponse)!;
        var result = new List<TripResponse>();

        foreach (var trip in json!.TripList.Trips)
        {
            var newTrip = new TripResponse();
            newTrip.Cancelled = trip.Cancelled != null && bool.Parse(trip.Cancelled);

            if (trip.Leg.GetValueKind() == JsonValueKind.Array)
            {
                newTrip.Legs.AddRange(JsonSerializer.Deserialize(trip.Leg.AsArray(), RejseplanenJsonContext.Default.ListTripLeg)!);
            } else if (trip.Leg.GetValueKind() == JsonValueKind.Object)
			{
                newTrip.Legs.Add(JsonSerializer.Deserialize(trip.Leg.AsObject(), RejseplanenJsonContext.Default.TripLeg)!);
			}
            else
            {
                throw new InvalidOperationException();
            }

            result.Add(newTrip);
		}

		return result;
	}
}
