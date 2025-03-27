using SimpLN.Models;

namespace SimpLN.Services.UtilityServices;

public class StateContainerService
{
	private InvoiceTypeResult? _invoiceResult;

	public InvoiceTypeResult? InvoiceResult
	{
		get => _invoiceResult;
		set => _invoiceResult = value;
	}

	public void ClearState()
	{
		_invoiceResult = null;
	}

	private CreateInvoiceResponse? _bolt11Invoice;

	public CreateInvoiceResponse Bolt11InvoiceResult
	{
		get => _bolt11Invoice;
		set => _bolt11Invoice = value;
	}

	private PayInvoiceResponse? _receipt;

	public PayInvoiceResponse Receipt
	{
		get => _receipt;
		set => _receipt = value;
	}
}