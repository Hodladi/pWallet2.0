using System.Text.RegularExpressions;
using SimpLN.Enums;
using SimpLN.Models;
using SimpLN.Services.PhoenixServices;

namespace SimpLN.Services.InvoiceServices
{
	public interface IInvoiceTypeService
	{
		Task<InvoiceTypeResult> IdentifyInvoiceType(string input);
	}

	public class InvoiceTypeService : IInvoiceTypeService
	{
		private readonly IWalletService _walletService;

		public InvoiceTypeService(IWalletService walletService)
		{
			_walletService = walletService;
		}
		public async Task<InvoiceTypeResult> IdentifyInvoiceType(string input)
		{
			var normalizedInput = input.ToLower();

			if (IsBitcoinAddress(normalizedInput))
			{
				var x = new InvoiceTypeResult(InvoiceType.BitcoinOnChain, normalizedInput);
				return x;
			}

			if (IsLightningInvoice(normalizedInput))
			{
				return new InvoiceTypeResult(InvoiceType.LightningInvoice, normalizedInput);
			}

			if (IsLnurl(normalizedInput))
			{
				var checkLnAuth = await _walletService.LnurlAuthAsync(normalizedInput);

				if (!checkLnAuth.Success)
				{
					return new InvoiceTypeResult(InvoiceType.LNURL, normalizedInput);
				}
				else
				{
					return new InvoiceTypeResult(InvoiceType.LNAuth, "Success");
				}
			}

			if (IsUnifiedPaymentRequest(normalizedInput))
			{
				return new InvoiceTypeResult(InvoiceType.UnifiedPaymentRequest, normalizedInput);
			}

			if (IsBolt12Offer(normalizedInput))
			{
				return new InvoiceTypeResult(InvoiceType.BOLT12Offer, normalizedInput);
			}

			if (IsBip353LightningAddress(normalizedInput))
			{
				return new InvoiceTypeResult(InvoiceType.BIP353LightningAddress, normalizedInput);
			}

			return new InvoiceTypeResult(InvoiceType.Unknown, normalizedInput);
		}

		private bool IsBitcoinAddress(string input)
		{
			var bitcoinRegex = new Regex(@"^(1|3|bc1)[a-z0-9]{25,39}$");
			return bitcoinRegex.IsMatch(input);
		}

		private bool IsLightningInvoice(string input)
		{
			var lightningRegex = new Regex(@"^ln(bc|tb|bcrt)[a-z0-9]+$");
			return lightningRegex.IsMatch(input);
		}

		private bool IsLnurl(string input)
		{
			var lnurlRegex = new Regex(@"^lnurl[a-z0-9]+$");
			return lnurlRegex.IsMatch(input);
		}

		private bool IsUnifiedPaymentRequest(string input)
		{
			var unifiedRegex = new Regex(@"^bitcoin:[a-z0-9]+(\?.*lightning=ln[a-z0-9]+)?$");
			return unifiedRegex.IsMatch(input);
		}

		private bool IsBolt12Offer(string input)
		{
			var bolt12Regex = new Regex(@"^lno[a-z0-9]+$");
			return bolt12Regex.IsMatch(input);
		}

		private bool IsBip353LightningAddress(string input)
		{
			var bip353Regex = new Regex(@"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$");
			return bip353Regex.IsMatch(input);
		}
	}
}