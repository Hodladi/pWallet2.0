using System.ComponentModel.DataAnnotations;
using SimpLN.Enums;

namespace SimpLN.Models;

public class PayModel
{
	public InvoiceType InvoiceType { get; set; }
	public string Invoice { get; set; }
	[Required]
	public long? Amount { get; set; }
	public long? Fee { get; set; }
	public string? Description { get; set; }
	public string? PaymentHash { get; set; }
}
