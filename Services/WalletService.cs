using SimpLN.Services.UserServices;

namespace SimpLN.Services;

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Models;

public interface IWalletService
{
	Task<BalanceResponse> GetBalanceAsync();
	Task<LnurlAuthResponse> LnurlAuthAsync(string lnurl);
	Task<List<IncomingPayment>> GetIncomingPaymentsAsync(PaymentQueryParameters queryParameters);
	Task<List<OutgoingPayment>> GetOutgoingPaymentsAsync(PaymentQueryParameters queryParameters);
	Task<string> GetBolt12OfferAsync();
	Task<string> GetLightningAddressAsync();
	Task<CreateInvoiceResponse> CreateBolt11InvoiceAsync(CreateInvoiceRequest requestData);
	Task<NodeInfoResponse> GetNodeInfoAsync();

}

public class WalletService : IWalletService
{
	private readonly HttpClient _httpClient;
	private readonly IConfigService _configService;

	public WalletService(IHttpClientFactory httpClientFactory, IConfigService configService)
	{
		_httpClient = httpClientFactory.CreateClient();
		_configService = configService;
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

	public async Task<BalanceResponse> GetBalanceAsync()
	{
		var request = await CreateAuthenticatedRequestAsync(HttpMethod.Get, "/getbalance");
		HttpResponseMessage? response;

		try
		{
			response = await _httpClient.SendAsync(request);

		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}

		if (response.IsSuccessStatusCode)
		{
			var result = await response.Content.ReadFromJsonAsync<BalanceResponse>();
			return result;
		}
		else
		{
			throw new Exception($"Failed to retrieve balance. Status code: {response.StatusCode}");
		}
	}

	public async Task<LnurlAuthResponse> LnurlAuthAsync(string lnurl)
	{
		var request = await CreateAuthenticatedRequestAsync(HttpMethod.Post, "/lnurlauth");

		var content = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("lnurl", lnurl)
		});
		request.Content = content;

		var response = await _httpClient.SendAsync(request);

		if (response.IsSuccessStatusCode)
		{
			return new LnurlAuthResponse { Success = true, Message = "Authentication success" };
		}
		else
		{
			return new LnurlAuthResponse { Success = false, Message = $"Failed to authenticate. Status code: {response.StatusCode}" };
		}
	}

	public async Task<List<IncomingPayment>> GetIncomingPaymentsAsync(PaymentQueryParameters queryParameters)
    {
        var queryString = BuildQueryString(queryParameters);
        var request = await CreateAuthenticatedRequestAsync(HttpMethod.Get, $"/payments/incoming{queryString}");
        
        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<IncomingPayment>>() ?? new List<IncomingPayment>();
        }
        else
        {
            throw new Exception($"Failed to retrieve incoming payments. Status code: {response.StatusCode}");
        }
    }

    public async Task<List<OutgoingPayment>> GetOutgoingPaymentsAsync(PaymentQueryParameters queryParameters)
    {
        var queryString = BuildQueryString(queryParameters);
        var request = await CreateAuthenticatedRequestAsync(HttpMethod.Get, $"/payments/outgoing{queryString}");
        
        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<OutgoingPayment>>() ?? new List<OutgoingPayment>();
        }
        else
        {
            throw new Exception($"Failed to retrieve outgoing payments. Status code: {response.StatusCode}");
        }
    }

    private string BuildQueryString(PaymentQueryParameters queryParameters)
    {
        var queryParams = new List<string>();

        if (queryParameters.From.HasValue)
            queryParams.Add($"from={queryParameters.From.Value}");
        
        if (queryParameters.To.HasValue)
            queryParams.Add($"to={queryParameters.To.Value}");
        
        if (queryParameters.Limit.HasValue)
            queryParams.Add($"limit={queryParameters.Limit.Value}");
        
        if (queryParameters.Offset.HasValue)
            queryParams.Add($"offset={queryParameters.Offset.Value}");
        
        if (queryParameters.All.HasValue)
            queryParams.Add($"all={queryParameters.All.Value.ToString().ToLower()}");
        
        if (!string.IsNullOrEmpty(queryParameters.ExternalId))
            queryParams.Add($"externalId={queryParameters.ExternalId}");

        return queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;
    }

    public async Task<string> GetBolt12OfferAsync()
    {
	    var request = await CreateAuthenticatedRequestAsync(HttpMethod.Get, "/getoffer");

	    var response = await _httpClient.SendAsync(request);

	    if (response.IsSuccessStatusCode)
	    {
		    return await response.Content.ReadAsStringAsync();
	    }
	    else
	    {
		    throw new Exception($"Failed to retrieve Bolt12 offer. Status code: {response.StatusCode}");
	    }
    }

    public async Task<string> GetLightningAddressAsync()
    {
	    var request = await CreateAuthenticatedRequestAsync(HttpMethod.Get, "/getlnaddress");

	    var response = await _httpClient.SendAsync(request);
	    var responseAsString = await response.Content.ReadAsStringAsync();

	    if (response.IsSuccessStatusCode)
	    {
		    if (responseAsString.StartsWith("\u20bf"))
		    {
			    responseAsString = responseAsString.Substring("\u20bf".Length);
		    }
		    return responseAsString;
	    }
	    else
	    {
		    throw new Exception($"Failed to retrieve Lightning address. Status code: {response.StatusCode}");
	    }
    }

    public async Task<CreateInvoiceResponse> CreateBolt11InvoiceAsync(CreateInvoiceRequest requestData)
    {
	    var request = await CreateAuthenticatedRequestAsync(HttpMethod.Post, "/createinvoice");

	    var parameters = new Dictionary<string, string>
	    {
		    { "description", requestData.Description ?? string.Empty },
		    { "amountSat", requestData.AmountSat.ToString() },
		    { "expirySeconds", requestData.ExpirySeconds.ToString() },
		    { "externalId", requestData.ExternalId ?? string.Empty },
		    { "webhookUrl", requestData.WebhookUrl ?? string.Empty }
	    };

	    request.Content = new FormUrlEncodedContent(parameters);

	    var response = await _httpClient.SendAsync(request);

	    if (response.IsSuccessStatusCode)
	    {

		    return await response.Content.ReadFromJsonAsync<CreateInvoiceResponse>();
	    }
	    else
	    {
		    throw new Exception($"Failed to create Bolt11 invoice. Status code: {response.StatusCode}");
	    }
    }

    public async Task<NodeInfoResponse> GetNodeInfoAsync()
    {
	    var request = await CreateAuthenticatedRequestAsync(HttpMethod.Get, "/getinfo");

	    var response = await _httpClient.SendAsync(request);

	    if (response.IsSuccessStatusCode)
	    {
		    var result = await response.Content.ReadFromJsonAsync<NodeInfoResponse>();
		    return result;
	    }
	    else
	    {
		    throw new Exception($"Failed to retrieve node info. Status code: {response.StatusCode}");
	    }
    }
}