using SimpLN.Enums;

namespace SimpLN.Models;

public class PayModel
{
	public InvoiceType InvoiceType { get; set; }
	public string Invoice { get; set; }
	public long Amount { get; set; }
	public long? Fee { get; set; }
	public string? Description { get; set; }
}
