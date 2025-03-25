namespace SimpLN.Models;

public class CreateInvoiceRequest
{
	public string? Description { get; set; }
	public string? DescriptionHash { get; set; }
	public long AmountSat { get; set; }
	public int ExpirySeconds { get; set; } = 3600; // Default to 1 hour
	public string? ExternalId { get; set; }
	public string? WebhookUrl { get; set; }
}

public class CreateInvoiceResponse
{
	public long AmountSat { get; set; }
	public string PaymentHash { get; set; } = string.Empty;
	public string Serialized { get; set; } = string.Empty;
}
