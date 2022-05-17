using Ardalis.ApiEndpoints;
using CosManagement.CQRS.Categories.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CosManagement.Endpoints.Category;

[Authorize]
public class DeleteCategory : EndpointBaseAsync
	.WithRequest<Guid>
	.WithActionResult
{
	private readonly IMediator _mediator;

	public DeleteCategory(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpDelete("api/categories/{id}")]
	[SwaggerOperation(
	Summary = "Delete category by id",
	Description = "Delete category by id",
	OperationId = "Categories.Delete",
	Tags = new[] { "CategoriesEndpoints" })]
	public override async Task<ActionResult> HandleAsync(Guid id, CancellationToken cancellationToken = default)
	{
		await _mediator.Send(new DeleteCategoryCommand { Id = id }, cancellationToken);
		return NoContent();
	}
}