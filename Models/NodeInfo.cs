namespace SimpLN.Models;

public class NodeInfoResponse
{
	public string NodeId { get; set; }
	public List<ChannelInfo> Channels { get; set; }
}

public class ChannelInfo
{
	public string State { get; set; }
	public string ChannelId { get; set; }
	public long BalanceSat { get; set; }
	public long InboundLiquiditySat { get; set; }
	public long CapacitySat { get; set; }
	public string FundingTxId { get; set; }
}
