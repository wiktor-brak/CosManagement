using CosManagement.Interfaces;
using MediatR;

namespace CosManagement.CQRS.Treatments.Commands.Delete;

public class DeleteTreatmentCommand : IRequest<Unit>, IIdParameter
{
	public Guid Id { get; set; }
}