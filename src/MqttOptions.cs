// See https://aka.ms/new-console-template for more information

namespace Rejseplanen2Mqtt.Client;
public class MqttOptions
{
	public int Port { get; set; }
	public bool UseTls { get; set; }
	public string Server { get; set; } = null!;
	public string CaCrt { get; set; } = null!;
	public string ClientCrt { get; set; } = null!;
	public string ClientKey { get; set; } = null!;
}
