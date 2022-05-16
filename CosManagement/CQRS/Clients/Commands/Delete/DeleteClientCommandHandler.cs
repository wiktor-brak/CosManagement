using CosManagement.CQRS.Common.Commands;
using CosManagement.Database;
using CosManagement.Entities;
using CosManagement.Interfaces;

namespace CosManagement.CQRS.Clients.Commands.Delete;

public class DeleteClientCommandHandler : DeleteBaseHandler<DeleteClientCommand, Client>
{
	public DeleteClientCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
	{ }
}