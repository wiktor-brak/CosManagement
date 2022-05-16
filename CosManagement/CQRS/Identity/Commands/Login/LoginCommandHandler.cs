using CosManagement.Exceptions;
using CosManagement.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CosManagement.CQRS.Identity.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, JwtResponse>
{
	private readonly UserManager<IdentityUser> _userManager;
	private readonly IConfiguration _configuration;

	public LoginCommandHandler(
		UserManager<IdentityUser> userManager,
		IConfiguration configuration)
	{
		_userManager = userManager;
		_configuration = configuration;
	}

	public async Task<JwtResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByNameAsync(request.Username);
		if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
		{
			var userRoles = await _userManager.GetRolesAsync(user);

			var authClaims = new List<Claim>
				{
					new Claim(ClaimTypes.NameIdentifier, user.Id),
				};

			foreach (var userRole in userRoles)
			{
				authClaims.Add(new Claim(ClaimTypes.Role, userRole));
			}

			var token = GetToken(authClaims);

			return new JwtResponse
			{
				Token = new JwtSecurityTokenHandler().WriteToken(token),
				Expiration = token.ValidTo
			};
		}

		throw new LoginFailException();
	}

	private JwtSecurityToken GetToken(List<Claim> authClaims)
	{
		var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

		var token = new JwtSecurityToken(
			issuer: _configuration["JWT:ValidIssuer"],
			audience: _configuration["JWT:ValidAudience"],
			expires: DateTime.Now.AddHours(3),
			claims: authClaims,
			signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
			);

		return token;
	}
}