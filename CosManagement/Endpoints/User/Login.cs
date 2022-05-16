using Ardalis.ApiEndpoints;
using CosManagement.CQRS.Identity.Commands;
using CosManagement.Identity.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CosManagement.Endpoints.User;

public class Login : EndpointBaseAsync
	.WithRequest<LoginCommand>
	.WithActionResult<JwtResponse>
{
	private readonly IMediator _mediator;

	public Login(
		IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost("api/login")]
	[SwaggerOperation(
	Summary = "Login by email and password",
	Description = "Login by email and password",
	OperationId = "Login",
	Tags = new[] { "IdentityEndpoints" })]
	public override async Task<ActionResult<JwtResponse>> HandleAsync([FromBody] LoginCommand command, CancellationToken cancellationToken = default)
	{
		return await _mediator.Send(command, cancellationToken);
	}
}