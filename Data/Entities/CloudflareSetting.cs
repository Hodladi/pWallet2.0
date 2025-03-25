namespace SimpLN.Data.Entities;

public class CloudflareSetting
{
	public int Id { get; set; }
	public string SiteKey { get; set; }
	public string ApiKey { get; set; }
	public string UserId { get; set; }
	public ApplicationUser ApplicationUser { get; set; }
}
