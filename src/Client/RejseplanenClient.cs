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

    public async Task<List<List<TripLeg>>> TripAsync(TripRequestOptions options)
	{
		var request = new HttpRequestMessage()
		{
			RequestUri = new Uri($"/bin/rest.exe/trip?originId={options.OriginId}&destId={options.DestId}&format=json", UriKind.Relative)
		};
		var response = await _httpClient.SendAsync(request);

		response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadFromJsonAsync(RejseplanenJsonContext.Default.TripResponse)!;
        var result = new List<List<TripLeg>>();

        foreach (var trip in json!.TripList.Trips)
        {
            if(trip.Leg.GetValueKind() == JsonValueKind.Array)
            {
                result.Add(JsonSerializer.Deserialize(trip.Leg.AsArray(), RejseplanenJsonContext.Default.ListTripLeg)!);
            } else if (trip.Leg.GetValueKind() == JsonValueKind.Object)
			    {
				    result.Add([JsonSerializer.Deserialize(trip.Leg.AsObject(), RejseplanenJsonContext.Default.TripLeg)!]);
			    } else
            {
                throw new InvalidOperationException();
            }
		}

		return result;
	}
}
