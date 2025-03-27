namespace SimpLN.Models;

public class PayOnChainRequest
{
	public long? AmountSat { get; set; }
	public string Address { get; set; }
	public long FeeRateSatByte { get; set; }
}