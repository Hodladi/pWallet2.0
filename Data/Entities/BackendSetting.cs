namespace SimpLN.Data.Entities;

public class BackendSetting
{
	public int Id { get; set; }
	public string BackendUrl { get; set; }
	public string ApiKey { get; set; }
	public string UserId { get; set; }
	public ApplicationUser ApplicationUser { get; set; }
}
