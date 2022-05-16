using CosManagement.Interfaces;
using MediatR;

namespace CosManagement.CQRS.Clients.Commands.Delete;

public class DeleteClientCommand : IRequest<Unit>, IIdParameter
{
	public Guid Id { get; set; }
}