using CloudFlare.Client.Api.Zones.DnsRecord;
using CloudFlare.Client.Enumerators;
using CloudFlare.Client;
using SimpLN.Services.UserServices;

public class CloudflareDnsService
{
    private readonly IConfigService _configService;

    public CloudflareDnsService(IConfigService configService)
    {
        _configService = configService;
    }

    public async Task<string> AddTxtRecord(string name, string content)
    {
        var cloudflareSettings = await _configService.GetCloudflareSettingsAsync();
        var client = new CloudFlareClient(cloudflareSettings.SiteKey);
        if (!content.StartsWith("bitcoin:?lno="))
        {
            content = "bitcoin:?lno=" + content;
        }

        var chunks = SplitIntoChunks(content, 255);

        var newRecord = new NewDnsRecord
        {
            Type = DnsRecordType.Txt,
            Name = $"{name}.user._bitcoin-payment",
            Content = string.Join("", chunks),
            Ttl = 1
        };

        var result = await client.Zones.DnsRecords.AddAsync(cloudflareSettings.ApiKey, newRecord);
        return result.Result.Id;
    }

    private IEnumerable<string> SplitIntoChunks(string str, int chunkSize)
    {
        yield return str.Substring(0, Math.Min(chunkSize, str.Length));

        for (int i = chunkSize; i < str.Length; i += chunkSize)
            yield return str.Substring(i, Math.Min(chunkSize, str.Length - i));
    }

    public async Task<string> GetCurrentCustomName(string bolt12Offer)
    {
        var cloudflareSettings = await _configService.GetCloudflareSettingsAsync();
        var client = new CloudFlareClient(cloudflareSettings.SiteKey);
        var records = await client.Zones.DnsRecords.GetAsync(cloudflareSettings.ApiKey);

        foreach (var record in records.Result)
        {
            if (record.Type == DnsRecordType.Txt && record.Content.Contains(bolt12Offer))
            {
                var parts = record.Name.Split('.');
                return parts[0];
            }
        }

        return string.Empty;
    }

    public async Task<string> GetDomainNameAsync()
    {
        try
        {
            var cloudflareSettings = await _configService.GetCloudflareSettingsAsync();
            var client = new CloudFlareClient(cloudflareSettings.SiteKey);
            var zone = await client.Zones.GetDetailsAsync(cloudflareSettings.ApiKey);
            return zone.Result.Name;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching domain name: {ex.Message}");
            return string.Empty;
        }
    }

    public async Task<string> AddOrUpdateTxtRecord(string name, string content)
    {
        var existingName = await GetCurrentCustomName(content);
        if (!string.IsNullOrEmpty(existingName))
        {
            await DeleteTxtRecord(existingName, content);
        }
        return await AddTxtRecord(name, content);
    }

    public async Task<bool> DeleteTxtRecord(string name, string content)
    {
	    var domainName = await GetDomainNameAsync();
        var cloudflareSettings = await _configService.GetCloudflareSettingsAsync();
        var client = new CloudFlareClient(cloudflareSettings.SiteKey);
        var records = await client.Zones.DnsRecords.GetAsync(cloudflareSettings.ApiKey);

        var recordToDelete = records.Result.FirstOrDefault(r =>
            r.Type == DnsRecordType.Txt &&
            r.Name.Equals($"{name}.user._bitcoin-payment.{domainName}", StringComparison.OrdinalIgnoreCase));

        if (recordToDelete != null)
        {
            var result = await client.Zones.DnsRecords.DeleteAsync(cloudflareSettings.ApiKey, recordToDelete.Id);
            return result.Success;
        }
        else
        {
            Console.WriteLine($"No matching DNS record found for deletion: Name={name}, Content={content}");
            return false;
        }
    }
}
