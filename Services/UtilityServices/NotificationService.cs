namespace SimpLN.Services.UtilityServices;

public class NotificationService
{

	public string NotificationMessage { get; private set; }
	public string NotificationStyle { get; private set; }

	public event Action OnChange;

	public void ShowNotification(string message)
	{
		NotificationMessage = message;
		NotificationStyle = "top: 0;";
		OnChange?.Invoke();
	}

	public void HideNotification()
	{
		NotificationStyle = "top: -100px; transition: top 0.5s ease-in-out;";
		OnChange?.Invoke();
	}
}