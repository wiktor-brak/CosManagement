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

	public override void MapAdditionalProperties(Appointment resource, UpdateAppointmentCommand request)
	{
		resource.Treatments = new();

		request.Dto?.TreatmentsIds.ForEach(treatmentId =>
		{
			var treatment = _context.Treatments.FirstOrDefault(t => t.Id == treatmentId);
			if (treatment is not null)
			{
				resource.Treatments.Add(treatment);
			}
		});
	}
}