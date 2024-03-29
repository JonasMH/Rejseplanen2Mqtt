﻿using System.Text.Json.Serialization;

namespace Rejseplanen2Mqtt.Client;

public class TripLegStation
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("type")]
	public string Type { get; set; } = null!;

    [JsonPropertyName("routeIdx")]
	public string? RouteIdx { get; set; }

    [JsonPropertyName("time")]
	public string Time { get; set; } = null!;

    [JsonPropertyName("date")]
	public string Date { get; set; } = null!;
    [JsonPropertyName("track")]
    public string? Track { get; set; }

    [JsonPropertyName("rtTrack")]
	public string? RealtimeTrack { get; set; }

    [JsonPropertyName("rtTime")]
	public string? RealtimeTime { get; set; }

    [JsonPropertyName("rtDate")]
	public string? RealtimeDate { get; set; }
}
