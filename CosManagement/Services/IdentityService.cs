using CosManagement.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosManagement.Services;

public class IdentityService : IIdentityService
{
	private readonly UserManager<IdentityUser> _userManager;

	public IdentityService(UserManager<IdentityUser> userManager)
	{
		_userManager = userManager;
	}

	public async Task<string> GetUserNameAsync(string userId)
	{
		var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

		return user.UserName;
	}
}