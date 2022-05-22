using AutoMapper;
using CosManagement.CQRS.Common;
using CosManagement.Database;
using CosManagement.Dtos.Appointments;
using CosManagement.Entities;
using CosManagement.Exceptions;
using CosManagement.Interfaces;

namespace CosManagement.CQRS.Appointments.Commands.Create;

public class CreateAppointmentCommandHandler : CreateBaseHandler<UpdateApptointmentCommand, CreateAppointmentDto, Appointment>
{
	private readonly ApplicationDbContext _context;

	public CreateAppointmentCommandHandler(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
		: base(context, mapper, currentUserService)
	{
		_context = context;
	}

	public override void AppendAdditionalProperty(Appointment resource, UpdateApptointmentCommand request)
	{
		resource.Treatments = new();

		request.TreatmentsIds.ForEach(treatmentId =>
		{
			var treatment = _context.Treatments.FirstOrDefault(t => t.Id == treatmentId);
			if (treatment is not null)
			{
				resource.Treatments.Add(treatment);
			}
		});
	}

	public override void AppendAdditionalValidation(Appointment resource)
	{
		var client = _context.Clients.FirstOrDefault(c => c.Id == resource.ClientId);

		if (client is null)
		{
			throw new NotFoundException($"{NotFoundException.MessagePrefix} client");
		}
	}
}