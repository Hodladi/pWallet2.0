﻿namespace SimpLN.Services.InvoiceServices;

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Models;
using Models.Config;
using UserServices;

public interface IInvoiceDecodeService
{
	Task<Bolt11Invoice?> DecodeBolt11InvoiceAsync(string invoice);
	Task<Bolt12Offer?> DecodeBolt12OfferAsync(string offer);
}

public class InvoiceDecodeService : IInvoiceDecodeService
{
	private readonly PhoenixBackendSettings _phoenixConfig;
	private readonly HttpClient _httpClient;

	public InvoiceDecodeService(IHttpClientFactory httpClientFactory, IOptions<PhoenixBackendSettings> phoenixOptions)
	{
		_phoenixConfig = phoenixOptions.Value;
		_httpClient = httpClientFactory.CreateClient();
	}

	private async Task<HttpRequestMessage> CreateAuthenticatedRequest(HttpMethod method, string path)
	{
		var apiHost = _phoenixConfig.ApiUrl;
		var password = _phoenixConfig.ApiPassword;

		var request = new HttpRequestMessage(method, $"{apiHost}{path}");
		var authValue = Convert.ToBase64String(Encoding.ASCII.GetBytes($":{password}"));
		request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authValue);

		return request;
	}


	public async Task<Bolt11Invoice?> DecodeBolt11InvoiceAsync(string invoice)
	{
		var request = await CreateAuthenticatedRequest(HttpMethod.Post, "/decodeinvoice");

		var content = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("invoice", invoice)
		});
		request.Content = content;

		HttpResponseMessage? response;

		try
		{
			response = await _httpClient.SendAsync(request);
		}
		catch (Exception e)
		{
			Console.WriteLine($"Error while decoding Bolt 11 invoice: {e.Message}");
			throw;
		}

		if (response.IsSuccessStatusCode)
		{
			try
			{
				var result = await response.Content.ReadFromJsonAsync<Bolt11Invoice>();
				return result;
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error parsing Bolt 11 invoice response: {e.Message}");
				throw;
			}
		}
		else
		{
			throw new Exception($"Failed to decode Bolt 11 invoice. Status code: {response.StatusCode}");
		}
	}

	public async Task<Bolt12Offer?> DecodeBolt12OfferAsync(string offer)
	{
		var request = await CreateAuthenticatedRequest(HttpMethod.Post, "/decodeoffer");

		// Add offer to the request body
		var content = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("offer", offer)
		});
		request.Content = content;

		HttpResponseMessage? response;

		try
		{
			response = await _httpClient.SendAsync(request);
		}
		catch (Exception e)
		{
			Console.WriteLine($"Error while decoding Bolt 12 offer: {e.Message}");
			throw;
		}

		if (response.IsSuccessStatusCode)
		{
			try
			{
				var result = await response.Content.ReadFromJsonAsync<Bolt12Offer>();
				return result;
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error parsing Bolt 12 offer response: {e.Message}");
				throw;
			}
		}
		else
		{
			throw new Exception($"Failed to decode Bolt 12 offer. Status code: {response.StatusCode}");
		}
	}
}
