using CosManagement.Common.Models;
using CosManagement.Dtos.Categories;
using CosManagement.Interfaces;
using MediatR;

namespace CosManagement.CQRS.Categories.Queries.GetCategories;

public class GetCategoriesWithPaginationQuery : IRequest<PaginatedList<GetCategoryDto>>, IGetAllQuery
{
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 10;
}