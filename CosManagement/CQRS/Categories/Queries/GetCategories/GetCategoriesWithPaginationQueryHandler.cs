using AutoMapper;
using CosManagement.CQRS.Common.Queries;
using CosManagement.Database;
using CosManagement.Dtos.Categories;
using CosManagement.Entities;
using CosManagement.Interfaces;

namespace CosManagement.CQRS.Categories.Queries.GetCategories;

public class GetCategoriesWithPaginationQueryHandler : GetAllBaseHandler<GetCategoriesWithPaginationQuery, GetCategoryDto, Category>
{
	public GetCategoriesWithPaginationQueryHandler(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
		: base(context, mapper, currentUserService)
	{
	}
}