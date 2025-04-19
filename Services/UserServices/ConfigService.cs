using System.Security.Claims;
using SimpLN.Data.Entities;
using SimpLN.Models.Config;
using SimpLN.Repositories;

namespace SimpLN.Services.UserServices;


public interface IConfigService
{
	Task<CloudflareSettingsModel> GetCloudflareSettingsAsync();
	Task UpdateCloudflareSettingsAsync(CloudflareSettingsModel model);
	Task UpdateCustomBolt12Async(string customBolt12);
	Task<string?> GetCustomBolt12Async();



}
public class ConfigService : IConfigService
{
	private readonly ConfigRepository _repository;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public ConfigService(ConfigRepository repository, IHttpContextAccessor httpContextAccessor)
	{
		_repository = repository;
		_httpContextAccessor = httpContextAccessor;
	}

	public async Task<CloudflareSettingsModel> GetCloudflareSettingsAsync()
	{
		var httpContext = _httpContextAccessor.HttpContext;
		if (httpContext.User.Identity?.IsAuthenticated ?? false)
		{
			var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			var setting = await _repository.GetCloudflareSettingAsync(userId);
			return MapEntityToModel(setting);
		}
		else
		{
			return new CloudflareSettingsModel();
		}
	}

	public async Task UpdateCustomBolt12Async(string customBolt12)
	{
		var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
		var user = await _repository.GetUserAsync(userId);
		user.CustomBolt12 = customBolt12;
		await _repository.UpdateUserAsync(user);
	}

	public async Task<string?> GetCustomBolt12Async()
	{
		Console.WriteLine($"HttpContext: {_httpContextAccessor.HttpContext}");
		Console.WriteLine($"User: {_httpContextAccessor.HttpContext?.User}");
		// Only this var is actually needed to fix the issue?
		var httpContext = _httpContextAccessor.HttpContext;
		if (httpContext == null || httpContext.User == null)
		{
			Console.WriteLine("Warning: HttpContext or User is null!");
			return null;
		}

		var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
		var user = await _repository.GetUserAsync(userId);
		return user.CustomBolt12;
	}

	public async Task UpdateCloudflareSettingsAsync(CloudflareSettingsModel model)
	{
		var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
		var entity = await _repository.GetCloudflareSettingAsync(userId);
		if (entity == null)
		{
			entity = new CloudflareSetting { UserId = userId };
		}
		MapModelToEntity(model, entity);
		await _repository.UpdateCloudflareSettingAsync(entity);
	}

	private CloudflareSettingsModel MapEntityToModel(CloudflareSetting entity)
	{
		if (entity == null)
		{
			return new CloudflareSettingsModel();
		}
		else
		{
			return new CloudflareSettingsModel
			{
				SiteKey = entity.SiteKey,
				ApiKey = entity.ApiKey,
				UserId = entity.UserId
			};
		}
	}

	private void MapModelToEntity(CloudflareSettingsModel model, CloudflareSetting entity)
	{
		entity.SiteKey = model.SiteKey;
		entity.ApiKey = model.ApiKey;
	}
}