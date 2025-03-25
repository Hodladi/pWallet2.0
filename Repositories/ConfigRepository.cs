using Microsoft.EntityFrameworkCore;
using SimpLN.Data.Entities;
using SimpLN.Data;
using SimpLN.Models.Config;

namespace SimpLN.Repositories;

public class ConfigRepository
{
	private readonly AppDbContext _context;

	public ConfigRepository(AppDbContext context)
	{
		_context = context;
	}

	public async Task<CloudflareSetting> GetCloudflareSettingAsync(string userId)
	{
		return await _context.CloudflareSettings
			.FirstOrDefaultAsync(cf => cf.UserId == userId);
	}

	public async Task<BackendSetting> GetBackendSettingAsync(string userId)
	{
		return await _context.BackendSettings
			.FirstOrDefaultAsync(bs => bs.UserId == userId);
	}

	public async Task<ApplicationUser> GetUserAsync(string userId)
	{
		return await _context.Users.FindAsync(userId);
	}

	public async Task UpdateUserAsync(ApplicationUser user)
	{
		_context.Users.Update(user);
		await _context.SaveChangesAsync();
	}

	public async Task UpdateCloudflareSettingAsync(CloudflareSetting setting)
	{
		if (setting.Id == 0)
		{
			_context.CloudflareSettings.Add(setting);
		}
		else
		{
			_context.CloudflareSettings.Update(setting);
		}
		await _context.SaveChangesAsync();
	}

	public async Task UpdateBackendSettingAsync(BackendSetting setting)
	{
		if (setting.Id == 0)
		{
			_context.BackendSettings.Add(setting);
		}
		else
		{
			_context.BackendSettings.Update(setting);
		}
		await _context.SaveChangesAsync();
	}
}

