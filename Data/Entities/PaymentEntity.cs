namespace SimpLN.Data.Entities;


public class PaymentEntity
{
	public int Id { get; set; }
	public string? PaymentHash { get; set; }
	public string? Message { get; set; }
	public string? InvoiceString { get; set; }
	public string UserId { get; set; }
	public ApplicationUser ApplicationUser { get; set; }
	public string? UserNote { get; set; }
}
