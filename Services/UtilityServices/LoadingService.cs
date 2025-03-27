namespace SimpLN.Services.UtilityServices;

public class LoadingService
{
	public event Action? OnChange;

	private readonly object _lock = new();
	private bool _isLoading;

	public bool IsLoading
	{
		get
		{
			lock (_lock)
			{
				return _isLoading;
			}
		}
		set
		{
			lock (_lock)
			{
				if (_isLoading != value)
				{
					_isLoading = value;
					NotifyStateChanged();
				}
			}
		}
	}

	public void StartLoading()
	{
		IsLoading = true;
	}

	public void StopLoading()
	{
		IsLoading = false;
	}

	private void NotifyStateChanged()
	{
		OnChange?.Invoke();
	}
}