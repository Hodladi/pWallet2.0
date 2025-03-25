namespace SimpLN.Models;

public class PaymentQueryParameters
{
	public long? From { get; set; }
	public long? To { get; set; }
	public int? Limit { get; set; }
	public int? Offset { get; set; }
	public bool? All { get; set; }
	public string? ExternalId { get; set; }
}

