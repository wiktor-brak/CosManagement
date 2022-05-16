using Ardalis.ApiEndpoints;
using CosManagement.CQRS.Categories.Queries.GetCategory;
using CosManagement.Dtos.Categories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CosManagement.Endpoints.Client;

[Authorize]
public class GetCategory : EndpointBaseAsync
	.WithRequest<Guid>
	.WithActionResult<GetCategoryDto>
{
	private readonly IMediator _mediator;

	public GetCategory(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("api/categories/{id}")]
	[SwaggerOperation(
	Summary = "Get one category by id (GUID)",
	Description = "Get one category by id (GUID)",
	OperationId = "Categories.Get",
	Tags = new[] { "CategoriesEndpoints" })]
	public override async Task<ActionResult<GetCategoryDto>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
	{
		return Ok(await _mediator.Send(new GetCategoryQuery { Id = id }, cancellationToken));
	}
}