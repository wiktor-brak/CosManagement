using CosManagement.CQRS.Common.Commands;
using CosManagement.Database;
using CosManagement.Entities;
using CosManagement.Interfaces;

namespace CosManagement.CQRS.Appointments.Commands.Delete;

public class DeleteAppointmentCommandHandler : DeleteBaseHandler<DeleteAppointmentCommand, Appointment>
{
	public DeleteAppointmentCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService)
		: base(context, currentUserService)
	{ }
}