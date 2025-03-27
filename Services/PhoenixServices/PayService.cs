using SimpLN.Models;

namespace SimpLN.Services.PhoenixServices;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UserServices;

public interface IPayService
{
	Task<PayInvoiceResponse> PayBolt11InvoiceAsync(PayInvoiceRequest request);
	Task<PayOfferResponse> PayBolt12OfferAsync(PayOfferRequest request);
	Task<PayLnAddressResponse> PayLightningAddressAsync(PayLnAddressRequest request);
	Task<string> PayOnChainAsync(PayOnChainRequest request);
	Task<LnurlPayResponse> PayLnurlAsync(LnurlPayRequest request);
}

public class PayService : IPayService
{
	private readonly IConfigService _configService;
	private readonly HttpClient _httpClient;

	public PayService(IHttpClientFactory httpClientFactory, IConfigService configService)
	{
		_configService = configService;
		_httpClient = httpClientFactory.CreateClient();
	}

	private async Task<string> GetApiHostAsync()
	{
		var backendSettings = await _configService.GetBackendSettingsAsync();
		return backendSettings.BackendUrl;
	}

	private async Task<string> GetPasswordAsync()
	{
		var backendSettings = await _configService.GetBackendSettingsAsync();
		return backendSettings.ApiKey;
	}

	private async Task<HttpRequestMessage> CreateAuthenticatedRequestAsync(HttpMethod method, string path)
	{
		var apiHost = await GetApiHostAsync();
		var password = await GetPasswordAsync();

		var request = new HttpRequestMessage(method, $"{apiHost}{path}");
		var authValue = Convert.ToBase64String(Encoding.ASCII.GetBytes($":{password}"));
		request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authValue);

		return request;
	}

	public async Task<PayInvoiceResponse> PayBolt11InvoiceAsync(PayInvoiceRequest request)
	{
		var httpRequest = await CreateAuthenticatedRequestAsync(HttpMethod.Post, "/payinvoice");

		var content = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("invoice", request.Invoice),
			new KeyValuePair<string, string>("amountSat", request.AmountSat?.ToString() ?? string.Empty),
		});

		httpRequest.Content = content;

		var response = await _httpClient.SendAsync(httpRequest);

		if (response.IsSuccessStatusCode)
		{
			return await response.Content.ReadFromJsonAsync<PayInvoiceResponse>();
		}
		else
		{
			throw new Exception($"Failed to pay Bolt11 invoice. Status code: {response.StatusCode}");
		}
	}

	public async Task<PayOfferResponse> PayBolt12OfferAsync(PayOfferRequest request)
	{
		var httpRequest = await CreateAuthenticatedRequestAsync(HttpMethod.Post, "/payoffer");

		var content = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("offer", request.Offer),
			new KeyValuePair<string, string>("amountSat", request.AmountSat.ToString()!),
			new KeyValuePair<string, string>("message", request.Message ?? string.Empty)
		});
		httpRequest.Content = content;

		var response = await _httpClient.SendAsync(httpRequest);

		if (response.IsSuccessStatusCode)
		{
			return await response.Content.ReadFromJsonAsync<PayOfferResponse>();
		}
		else
		{
			throw new Exception($"Failed to pay Bolt12 offer.");
		}
	}

	public async Task<PayLnAddressResponse> PayLightningAddressAsync(PayLnAddressRequest request)
	{
		var httpRequest = await CreateAuthenticatedRequestAsync(HttpMethod.Post, "/paylnaddress");

		var content = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("address", request.Address),
			new KeyValuePair<string, string>("amountSat", request.AmountSat?.ToString()!),
			new KeyValuePair<string, string>("message", request.Message ?? string.Empty)
		});
		httpRequest.Content = content;

		var response = await _httpClient.SendAsync(httpRequest);

		if (response.IsSuccessStatusCode)
		{
			return await response.Content.ReadFromJsonAsync<PayLnAddressResponse>();
		}
		else
		{
			throw new Exception($"Failed to pay Lightning address. Status code: {response.StatusCode}");
		}
	}

	public async Task<string> PayOnChainAsync(PayOnChainRequest request)
	{
		var httpRequest = await CreateAuthenticatedRequestAsync(HttpMethod.Post, "/sendtoaddress");

		var content = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("address", request.Address),
			new KeyValuePair<string, string>("amountSat", request.AmountSat.ToString()),
			new KeyValuePair<string, string>("feerateSatByte", request.FeeRateSatByte.ToString())
		});
		httpRequest.Content = content;

		var response = await _httpClient.SendAsync(httpRequest);

		if (response.IsSuccessStatusCode)
		{
			return await response.Content.ReadAsStringAsync();
		}
		else
		{
			throw new Exception($"Failed to send on-chain payment.");
		}
	}

	public async Task<LnurlPayResponse> PayLnurlAsync(LnurlPayRequest request)
	{
		var httpRequest = await CreateAuthenticatedRequestAsync(HttpMethod.Post, "/lnurlpay");

		var content = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("lnurl", request.Lnurl),
			new KeyValuePair<string, string>("amountSat", request.AmountSat.ToString()),
			new KeyValuePair<string, string>("message", request.Message ?? string.Empty)
		});
		httpRequest.Content = content;

		var response = await _httpClient.SendAsync(httpRequest);

		if (response.IsSuccessStatusCode)
		{
			return await response.Content.ReadFromJsonAsync<LnurlPayResponse>();
		}
		else
		{
			throw new Exception($"Failed to pay LNURL.");
		}
	}
}
