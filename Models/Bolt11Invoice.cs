namespace SimpLN.Models;


public class Bolt11Invoice
{
	public string Chain { get; set; }
	public long Amount { get; set; }
	public string PaymentHash { get; set; }
	public string Description { get; set; }
	public int MinFinalCltvExpiryDelta { get; set; }
	public string PaymentSecret { get; set; }
	public string PaymentMetadata { get; set; }
	public List<List<ExtraHop>> ExtraHops { get; set; }
	public Features Features { get; set; }
	public long TimestampSeconds { get; set; }
}

public class ExtraHop
{
	public string NodeId { get; set; }
	public string ShortChannelId { get; set; }
	public long FeeBase { get; set; }
	public long FeeProportionalMillionths { get; set; }
	public int CltvExpiryDelta { get; set; }
}

public class Features
{
	public Dictionary<string, string> Activated { get; set; }
	public List<string> Unknown { get; set; }
}

public class PayInvoiceRequest
{
	public string Invoice { get; set; }
	public long? AmountSat { get; set; }
}

public class PayInvoiceResponse
{
	public long? RecipientAmountSat { get; set; }
	public long? RoutingFeeSat { get; set; }
	public string? PaymentId { get; set; }
	public string? PaymentHash { get; set; }
	public string? PaymentPreimage { get; set; }
}
