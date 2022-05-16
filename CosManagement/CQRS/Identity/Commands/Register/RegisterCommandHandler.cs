using CosManagement.Database;
using CosManagement.Entities;
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
	private readonly ApplicationDbContext _context;

	public RegisterCommandHandler(
		UserManager<IdentityUser> userManager,
		ApplicationDbContext context
		)
	{
		_userManager = userManager;
		_context = context;
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

		SeedUserData(user.Id);

		return Unit.Value;
	}

	private async void SeedUserData(string id)
	{
		_context.Categories.Add(new Category
		{
			Name = "Common",
			Id = Guid.NewGuid(),
			OwnerId = id
		});

		await _context.SaveChangesAsync();
	}
}