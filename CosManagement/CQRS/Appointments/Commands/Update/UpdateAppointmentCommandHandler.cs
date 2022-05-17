using AutoMapper;
using CosManagement.CQRS.Common.Commands;
using CosManagement.Database;
using CosManagement.Dtos.Appointments;
using CosManagement.Entities;
using CosManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CosManagement.CQRS.Appointments.Commands.Update;

public class UpdateAppointmentCommandHandler : UpdateBaseHandler<UpdateAppointmentCommand, Appointment, UpdateAppointmentDto>
{
	private readonly ApplicationDbContext _context;

	public UpdateAppointmentCommandHandler(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
		: base(context, mapper, currentUserService)
	{
		_context = context;
	}

	public override IQueryable<Appointment> GetResources()
	{
		return _context.Appointments
			.Include(a => a.Client)
			.Include(a => a.Treatments);
	}
}