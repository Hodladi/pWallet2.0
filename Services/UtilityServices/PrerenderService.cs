namespace SimpLN.Services.UtilityServices;

public interface IPrerenderService
{
	public bool IsPrerendering { get; set; }
}
public class PrerenderService : IPrerenderService
{
	public bool IsPrerendering { get; set; } = true;
}
