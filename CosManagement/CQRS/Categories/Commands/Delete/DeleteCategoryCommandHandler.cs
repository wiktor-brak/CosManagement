using CosManagement.CQRS.Common.Commands;
using CosManagement.Database;
using CosManagement.Entities;
using CosManagement.Interfaces;

namespace CosManagement.CQRS.Categories.Commands.Delete;

public class DeleteCategoryCommandHandler : DeleteBaseHandler<DeleteCategoryCommand, Category>
{
	public DeleteCategoryCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService)
		: base(context, currentUserService)
	{ }
}