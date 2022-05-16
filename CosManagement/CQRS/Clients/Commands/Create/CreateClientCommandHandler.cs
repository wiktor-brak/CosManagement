using AutoMapper;
using CosManagement.CQRS.Common;
using CosManagement.Database;
using CosManagement.Dtos.Clients;
using CosManagement.Entities;
using CosManagement.Interfaces;

namespace CosManagement.CQRS.Clients.Commands.Create;

public class CreateClientCommandHandler : CreateBaseHandler<CreateClientCommand, CreateClientDto, Client>
{
	public CreateClientCommandHandler(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
		: base(context, mapper, currentUserService)
	{ }
}