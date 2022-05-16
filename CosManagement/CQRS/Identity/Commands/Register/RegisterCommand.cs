using MediatR;

namespace CosManagement.CQRS.Identity.Commands;

public class RegisterCommand : IRequest
{
	public string? Email { get; set; }

	public string? Password { get; set; }

	public string? ConfirmPassoword { get; set; }
}