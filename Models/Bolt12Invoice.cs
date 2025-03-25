namespace SimpLN.Models;

public class Bolt12Offer
{
	public string Chain { get; set; }
	public List<string> ChainHashes { get; set; }
	public List<Path> Path { get; set; }
}

public class Path
{
	public IntroductionNodeId IntroductionNodeId { get; set; }
	public string BlindingKey { get; set; }
	public List<BlindedNode> BlindedNodes { get; set; }
}

public class IntroductionNodeId
{
	public string PublicKey { get; set; }
}

public class BlindedNode
{
	public string BlindedPublicKey { get; set; }
	public string EncryptedPayload { get; set; }
}

public class PayOfferRequest
{
	public string Offer { get; set; }
	public long? AmountSat { get; set; }
	public string? Message { get; set; }
}

public class PayOfferResponse : PayInvoiceResponse
{
}