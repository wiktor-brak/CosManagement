using AutoMapper;
using CosManagement.CQRS.Common.Queries;
using CosManagement.Database;
using CosManagement.Dtos.Categories;
using CosManagement.Entities;
using CosManagement.Interfaces;

namespace CosManagement.CQRS.Categories.Queries.GetCategory;

public class GetCategoryQueryHandler : GetBaseHandler<GetCategoryQuery, GetCategoryDto, Category>
{
	public GetCategoryQueryHandler(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService) : base(context, mapper, currentUserService)
	{ }
}