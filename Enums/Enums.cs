using System.ComponentModel;

namespace SimpLN.Enums;

public enum TransactionType
{
	[Description("Receive")]
	Receive = 1,
	[Description("Send")]
	Send = 2
}

public enum InvoiceType
{
	BitcoinOnChain,
	LightningInvoice,
	LNURL,
	UnifiedPaymentRequest,
	BOLT12Offer,
	BIP353LightningAddress,
	LNAuth,
	Unknown
}