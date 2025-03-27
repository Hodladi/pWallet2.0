namespace SimpLN.Services.BitcoinPrice;

using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

public interface IBitcoinPriceService
{
	Task<(double?, double?)> GetBitcoinPriceInUsdAndEurAsync();
}

public class BitcoinPriceService : IBitcoinPriceService, IHostedService, IDisposable
{
	private readonly HttpClient _http;
	private readonly ILogger<BitcoinPriceService> _logger;
	private Timer? _timer;
	private double? _cachedUsdPrice;
	private double? _cachedEurPrice;
	private DateTime _lastUpdateTime = DateTime.MinValue;
	private readonly TimeSpan _updateInterval = TimeSpan.FromMinutes(16);

	public BitcoinPriceService(HttpClient http, ILogger<BitcoinPriceService> logger)
	{
		_http = http;
		_logger = logger;
	}

	public Task StartAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("Bitcoin Price Service started.");
		_timer = new Timer(UpdatePrice, null, TimeSpan.Zero, _updateInterval);
		return Task.CompletedTask;
	}

	public async Task<(double?, double?)> GetBitcoinPriceInUsdAndEurAsync()
	{
		if (DateTime.Now - _lastUpdateTime < _updateInterval)
		{
			return (_cachedUsdPrice, _cachedEurPrice);
		}

		await UpdatePriceAsync();
		return (_cachedUsdPrice, _cachedEurPrice);
	}

	private async Task UpdatePriceAsync()
	{
		try
		{
			var response = await _http.GetAsync("https://blockchain.info/ticker");

			if (!response.IsSuccessStatusCode)
			{
				return;
			}

			var jsonResponse = await response.Content.ReadAsStringAsync();
			var tickerData = JsonSerializer.Deserialize<Dictionary<string, CurrencyTicker>>(jsonResponse);

			if (tickerData != null && tickerData.TryGetValue("USD", out var usdTicker) && tickerData.TryGetValue("EUR", out var eurTicker))
			{
				_cachedEurPrice = eurTicker.Last;
				_cachedUsdPrice = usdTicker.Last;
				_lastUpdateTime = DateTime.Now;
			}
			else
			{
			}
		}
		catch (Exception ex)
		{
		}
	}

	private void UpdatePrice(object? state)
	{
		_ = UpdatePriceAsync();
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		_timer?.Change(Timeout.Infinite, 0);
		return Task.CompletedTask;
	}

	public void Dispose()
	{
		_timer?.Dispose();
	}
}

public class CurrencyTicker
{
	[JsonPropertyName("last")]
	public double Last { get; set; }
	[JsonPropertyName("symbol")]
	public string Symbol { get; set; } = string.Empty;
}

