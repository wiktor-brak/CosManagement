using CosManagement.Interfaces;
using MediatR;

namespace CosManagement.CQRS.Appointments.Commands.Delete;

public class DeleteAppointmentCommand : IRequest<Unit>, IIdParameter
{
	public Guid Id { get; set; }
}