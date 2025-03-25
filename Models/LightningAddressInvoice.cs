namespace SimpLN.Models;

public class PayLnAddressRequest
{
	public string Address { get; set; }
	public long? AmountSat { get; set; }
	public string? Message { get; set; }
}

public class PayLnAddressResponse : PayInvoiceResponse
{
}