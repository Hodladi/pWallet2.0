namespace SimpLN.Models.TransactionHistory;

public class OutgoingPaymentDetails
{
	public string Type { get; set; }
	public string SubType { get; set; }
	public string PaymentId { get; set; }
	public string PaymentHash { get; set; }
	public string Preimage { get; set; }
	public bool IsPaid { get; set; }
	public int Sent { get; set; }
	public int Fees { get; set; }
	public string? Invoice { get; set; }
	public long CompletedAt { get; set; }
	public long CreatedAt { get; set; }
	public string? TxId { get; set; }
}