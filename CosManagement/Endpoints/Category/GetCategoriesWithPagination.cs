using Ardalis.ApiEndpoints;
using CosManagement.Common.Models;
using CosManagement.CQRS.Categories.Queries.GetCategories;
using CosManagement.Dtos.Categories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CosManagement.Endpoints.Category;

[Authorize]
public class GetCategoriesWithPagination : EndpointBaseAsync
	.WithRequest<GetCategoriesWithPaginationQuery>
	.WithActionResult<PaginatedList<GetCategoryDto>>
{
	private readonly IMediator _mediator;

	public GetCategoriesWithPagination(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("api/categories")]
	[SwaggerOperation(
	Summary = "Get paginated list of categories  with 10 entires",
	Description = "Get paginated list of categories with 10 entires",
	OperationId = "Categories.GetAll",
	Tags = new[] { "CategoriesEndpoints" })]
	public override async Task<ActionResult<PaginatedList<GetCategoryDto>>> HandleAsync([FromQuery] GetCategoriesWithPaginationQuery query, CancellationToken cancellationToken = default)
	{
		return Ok(await _mediator.Send(query, cancellationToken));
	}
}