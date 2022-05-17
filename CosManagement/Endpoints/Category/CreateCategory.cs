using Ardalis.ApiEndpoints;
using CosManagement.CQRS.Categories.Commands.Create;
using CosManagement.CQRS.Clients.Commands.Create;
using CosManagement.Dtos.Categories;
using CosManagement.Dtos.Clients;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CosManagement.Endpoints.Category;

[Authorize]
public class CreateCategory : EndpointBaseAsync
	.WithRequest<CreateCategoryCommand>
	.WithActionResult<CreateCategoryDto>
{
	private readonly IMediator _mediator;

	public CreateCategory(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost("api/categories")]
	[SwaggerOperation(
	Summary = "Creates new category",
	Description = "Creates new category",
	OperationId = "Category.Create",
	Tags = new[] { "CategoriesEndpoints" })]
	public override async Task<ActionResult<CreateCategoryDto>> HandleAsync(CreateCategoryCommand command, CancellationToken cancellationToken = default)
	{
		var category = await _mediator.Send(command, cancellationToken);
		return CreatedAtRoute("GetClient", new { id = category.Id }, category);
	}
}