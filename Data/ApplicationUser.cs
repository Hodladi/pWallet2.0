using Microsoft.AspNetCore.Identity;
using SimpLN.Data.Entities;

namespace SimpLN.Data;

public class ApplicationUser : IdentityUser
{
	public CloudflareSetting CloudflareSetting { get; set; }
	public BackendSetting BackendSetting { get; set; }
	public string? CustomBolt12 { get; set; }
}
