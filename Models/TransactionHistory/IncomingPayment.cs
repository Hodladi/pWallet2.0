namespace SimpLN.Models.TransactionHistory;

public class IncomingPayment
{
	public string Type { get; set; }
	public string SubType { get; set; }
	public string PaymentHash { get; set; }
	public string Preimage { get; set; }
	public string? ExternalId { get; set; }
	public string? Description { get; set; }
	public string? Invoice { get; set; }
	public bool IsPaid { get; set; }
	public long ReceivedSat { get; set; }
	public long Fees { get; set; }
	public long CompletedAt { get; set; }
	public long CreatedAt { get; set; }
	public string? PayerNote { get; set; }
	public string? PayerKey { get; set; }
	public bool GotMessage { get; set; }
}
