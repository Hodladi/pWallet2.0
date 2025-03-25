using SimpLN.Enums;

namespace SimpLN.Models;

public class InvoiceTypeResult
{
	public InvoiceType Type { get; }
	public string Value { get; }

	public InvoiceTypeResult(InvoiceType type, string value)
	{
		Type = type;
		Value = value;
	}
}
