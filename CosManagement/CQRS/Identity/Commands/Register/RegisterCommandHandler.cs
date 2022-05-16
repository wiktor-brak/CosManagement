using CosManagement.Exceptions;
using CosManagement.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosManagement.CQRS.Identity.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
{
	private readonly UserManager<IdentityUser> _userManager;

	public RegisterCommandHandler(UserManager<IdentityUser> userManager)
	{
		_userManager = userManager;
	}

	public async Task<Unit> Handle(RegisterCommand request, CancellationToken cancellationToken)
	{
		var userExists = await _userManager.FindByNameAsync(request.Email);
		if (userExists != null)
			throw new UserExitstsException();

		IdentityUser user = new()
		{
			Email = request.Email,
			SecurityStamp = Guid.NewGuid().ToString(),
			UserName = request.Email
		};

		var result = await _userManager.CreateAsync(user, request.Password);

		if (!result.Succeeded)
			throw new InternalServerException();

		return Unit.Value;
	}
}