using Ardalis.ApiEndpoints;
using CosManagement.CQRS.Identity.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CosManagement.Endpoints.User;

public class Register : EndpointBaseAsync
	.WithRequest<RegisterCommand>
	.WithoutResult
{
	private readonly IMediator _mediator;

	public Register(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost("api/register")]
	[SwaggerOperation(
	Summary = "Register by email and password",
	Description = "Register by email and password",
	OperationId = "Register",
	Tags = new[] { "IdentityEndpoints" })]
	public override async Task<ActionResult> HandleAsync([FromBody] RegisterCommand command, CancellationToken cancellationToken = default)
	{
		await _mediator.Send(command, cancellationToken);
		return StatusCode(201);
	}
}