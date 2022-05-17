using Ardalis.ApiEndpoints;
using CosManagement.Attributes;
using CosManagement.CQRS.Categories.Commands.Update;
using CosManagement.Dtos.Categories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CosManagement.Endpoints.Category;

public class UpdateCategoryRequest
{
	[FromRoute(Name = "id")] public Guid Id { get; set; }
	[FromBody] public UpdateCategoryDto? CategoryDto { get; set; }
}

[Authorize]
public class UpdateCategory : EndpointBaseAsync
	.WithRequest<UpdateCategoryRequest>
	.WithActionResult
{
	private readonly IMediator _mediator;

	public UpdateCategory(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPut("api/categories/{id}")]
	[SwaggerOperation(
	Summary = "Update category by id",
	Description = "Update category by id",
	OperationId = "Categories.Update",
	Tags = new[] { "CategoriesEndpoints" })]
	public override async Task<ActionResult> HandleAsync([FormMultiSource] UpdateCategoryRequest request, CancellationToken cancellationToken = default)
	{
		await _mediator.Send(new UpdateCategoryCommand { Id = request.Id, Dto = request.CategoryDto }, cancellationToken);
		return Ok();
	}
}