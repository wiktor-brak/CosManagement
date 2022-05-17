using AutoMapper;
using CosManagement.CQRS.Common.Commands;
using CosManagement.Database;
using CosManagement.Dtos.Categories;
using CosManagement.Entities;
using CosManagement.Interfaces;

namespace CosManagement.CQRS.Categories.Commands.Update;

public class UpdateCategoryCommandHandler : UpdateBaseHandler<UpdateCategoryCommand, Category, UpdateCategoryDto>
{
	public UpdateCategoryCommandHandler(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
		: base(context, mapper, currentUserService)
	{ }
}