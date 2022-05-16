using CosManagement.Dtos.Clients;
using CosManagement.Interfaces;
using MediatR;

namespace CosManagement.CQRS.Clients.Commands.Update;

public class UpdateClientCommand : IRequest<Unit>, IMultiSourceResponse<UpdateClientDto>
{
	public Guid Id { get; set; }
	public UpdateClientDto? Dto { get; set; }
}