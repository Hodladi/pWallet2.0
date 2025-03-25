using System.ComponentModel.DataAnnotations;

namespace SimpLN.Models.Config;

public class CloudflareSettingsModel
{
	public int Id { get; set; }
	[Required]
	public string SiteKey { get; set; }
	[Required]
	public string ApiKey { get; set; }

	public string UserId { get; set; }
	// Add other properties as needed
}