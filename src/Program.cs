﻿// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Options;
using MQTTnet.Client;
using OpenTelemetry.Metrics;
using Rejseplanen2Mqtt.Client;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using ToMqttNet;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSimpleConsole(options =>
{
	options.IncludeScopes = true;
	options.SingleLine = true;
	options.TimestampFormat = "HH:mm:ss ";
});
builder.Services.AddOptions<MqttOptions>().BindConfiguration("MqttConnectionOptions");
builder.Services.AddOptions<RejseplanenToMqttOptions>().BindConfiguration("RejseplanenToMqttOptions");

builder.Services.AddHealthChecks();
builder.Services.AddOpenTelemetry()
	.WithMetrics(builder =>
	{
		builder.AddPrometheusExporter();
		builder.AddMeter("System.Net.Http",
						 "Microsoft.AspNetCore.Hosting",
						 "Microsoft.AspNetCore.Server.Kestrel");
	});

builder.Services.AddTransient<RejseplanenClient>(x => new RejseplanenClient(new HttpClient(), new RejseplanenClientOptions()));
builder.Services.AddMqttConnection()
	.Configure<IOptions<MqttOptions>>((options, mqttConfI) =>
	{
		var mqttConf = mqttConfI.Value;
		options.NodeId = "rejseplanen";
        options.OriginConfig = new HomeAssistantDiscoveryNet.MqttDiscoveryConfigOrigin
        {
            Name = "rejseplanen2mqtt",
            SoftwareVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
            SupportUrl = "https://github.com/JonasMH/Rejseplanen2Mqtt"
        };

		var tcpOptions = new MqttClientTcpOptions
		{
			Server = mqttConf.Server,
			Port = mqttConf.Port,
		};

		if (mqttConf.UseTls)
		{
			var caCrt = new X509Certificate2(mqttConf.CaCrt);
			var clientCrt = X509Certificate2.CreateFromPemFile(mqttConf.ClientCrt, mqttConf.ClientKey);


			tcpOptions.TlsOptions = new MqttClientTlsOptions
			{
				UseTls = true,
				SslProtocol = System.Security.Authentication.SslProtocols.Tls12,
				ClientCertificatesProvider = new DefaultMqttCertificatesProvider(new List<X509Certificate>()
			{
				clientCrt, caCrt
			}),
				CertificateValidationHandler = (certContext) =>
				{
					X509Chain chain = new X509Chain();
					chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
					chain.ChainPolicy.RevocationFlag = X509RevocationFlag.ExcludeRoot;
					chain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag;
					chain.ChainPolicy.VerificationTime = DateTime.Now;
					chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 0, 0);
					chain.ChainPolicy.CustomTrustStore.Add(caCrt);
					chain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;

					// convert provided X509Certificate to X509Certificate2
					var x5092 = new X509Certificate2(certContext.Certificate);

					return chain.Build(x5092);
				}
			};
		}


		options.ClientOptions.ChannelOptions = tcpOptions;
	});

builder.Services.AddHostedService<RejsePlanenToMqttBackgroundService>();

var app = builder.Build();

app.MapHealthChecks("/health");
app.MapPrometheusScrapingEndpoint("/metrics");

app.Run();
