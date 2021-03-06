using CosManagement.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CosManagement.Services;

public class IdentityService : IIdentityService
{
	private readonly UserManager<IdentityUser> _userManager;

	public IdentityService(UserManager<IdentityUser> userManager)
	{
		_userManager = userManager;
	}

	public async Task<string> GetUserNameAsync(string? userId)
	{
		var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

		if (user is null)
		{
			return "Unknown";
		}

		return user.UserName;
	}
}