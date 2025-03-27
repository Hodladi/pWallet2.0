namespace SimpLN.Models.TransactionHistory;

public class OutgoingPayment
{
	public string Type { get; set; }
	public string SubType { get; set; }
	public string PaymentId { get; set; }
	public string? TxId { get; set; }
	public string? PaymentHash { get; set; }
	public string? Preimage { get; set; }
	public bool IsPaid { get; set; }
	public long Sent { get; set; }
	public long Fees { get; set; }
	public long CompletedAt { get; set; }
	public long CreatedAt { get; set; }
	public string? Message { get; set; }
	public string? Recipient { get; set; }
}