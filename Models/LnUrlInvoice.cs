namespace SimpLN.Models;

public class LnurlPayRequest
{
	public long? AmountSat { get; set; }
	public string Lnurl { get; set; }
	public string? Message { get; set; }
}

public class LnurlPayResponse : PayInvoiceResponse
{
}