using AutoMapper;
using CosManagement.CQRS.Common.Commands;
using CosManagement.Database;
using CosManagement.Dtos.Clients;
using CosManagement.Entities;
using CosManagement.Interfaces;

namespace CosManagement.CQRS.Clients.Commands.Update;

public class UpdateClientCommandHandler : UpdateBaseHandler<UpdateClientCommand, Client, UpdateClientDto>
{
	public UpdateClientCommandHandler(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService) : base(context, mapper, currentUserService)
	{ }
}