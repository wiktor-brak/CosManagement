using AutoMapper;
using CosManagement.CQRS.Common;
using CosManagement.Database;
using CosManagement.Dtos.Categories;
using CosManagement.Entities;
using CosManagement.Interfaces;

namespace CosManagement.CQRS.Categories.Commands.Create;

public class CreateCategoryCommandHandler : CreateBaseHandler<CreateCategoryCommand, CreateCategoryDto, Category>
{
	public CreateCategoryCommandHandler(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
		: base(context, mapper, currentUserService)
	{
	}
}