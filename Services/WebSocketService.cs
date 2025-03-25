using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using SimpLN.Services.UserServices;

namespace SimpLN.Services;

public class WebSocketService : IDisposable
{
	private readonly IConfigService _configService;
	private string _apiHost;
    private string _apiPassword;
    private ClientWebSocket? _webSocketClient;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public WebSocketService(IConfigService configService)
    {
	    _configService = configService;
    }

    public async Task StartWebSocketAsync()
    {
	    var backendSettings = await _configService.GetBackendSettingsAsync();
	    _apiHost = backendSettings.BackendUrl;
	    _apiPassword = backendSettings.ApiKey;

	    await _semaphore.WaitAsync();
	    try
	    {
		    if (_webSocketClient != null)
		    {
			    return;
		    }
		    
		    var webSocketUrl = new Uri(_apiHost.Replace("http://", "ws://").Replace("https://", "wss://") + "/websocket");
		   var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(":" + _apiPassword));

		    _webSocketClient = new ClientWebSocket();
		    _webSocketClient.Options.SetRequestHeader("Authorization", $"Basic {token}");
		    await _webSocketClient.ConnectAsync(webSocketUrl, CancellationToken.None);

		    await ReceiveMessagesAsync();
	    }
	    finally
	    {
		    _semaphore.Release();
	    }
    }

    private async Task ReceiveMessagesAsync()
    {
        var buffer = new byte[1024 * 4];
        while (_webSocketClient?.State == WebSocketState.Open)
        {
            try
            {
                var result = await _webSocketClient.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    await HandleMessageAsync(message);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await HandleConnectionCloseAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error receiving message: {ex.Message}");
                await HandleConnectionCloseAsync();
            }
        }
    }

    private async Task HandleConnectionCloseAsync()
    {
        if (_webSocketClient != null)
        {
            await _webSocketClient.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            _webSocketClient = null;
            await StartWebSocketAsync();
        }
    }
    public event Action<PaymentReceivedMessage>? OnPaymentReceived;

    private async Task HandleMessageAsync(string message)
    {
	    var paymentData = JsonSerializer.Deserialize<PaymentReceivedMessage>(message);
	    if (paymentData?.Type == "payment_received")
	    {
		    OnPaymentReceived?.Invoke(paymentData);
	    }
    }

	

    public void Dispose()
    {
        _webSocketClient?.Dispose();
    }
}

public class PaymentReceivedMessage
{
	[JsonPropertyName("type")]
	public string Type { get; set; }

	[JsonPropertyName("amountSat")]
	public int AmountSat { get; set; }

	[JsonPropertyName("paymentHash")]
	public string PaymentHash { get; set; }

	[JsonPropertyName("externalId")]
	public string ExternalId { get; set; }

	[JsonPropertyName("timestamp")]
	public long Timestamp { get; set; }
}
