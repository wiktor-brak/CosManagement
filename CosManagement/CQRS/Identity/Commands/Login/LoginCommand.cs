using CosManagement.Identity.Models;
using MediatR;

namespace CosManagement.CQRS.Identity.Commands;

public class LoginCommand : IRequest<JwtResponse>
{
	public string? Username { get; set; }

	public string? Password { get; set; }
}