using AutoMapper;
using CosManagement.CQRS.Common;
using CosManagement.Database;
using CosManagement.Dtos.Appointments;
using CosManagement.Entities;
using CosManagement.Exceptions;
using CosManagement.Interfaces;

namespace CosManagement.CQRS.Appointments.Commands.Create;

public class CreateAppointmentCommandHandler : CreateBaseHandler<CreateAppointmentCommand, CreateAppointmentDto, Appointment>
{
	private readonly ApplicationDbContext _context;

	public CreateAppointmentCommandHandler(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
		: base(context, mapper, currentUserService)
	{
		_context = context;
	}

	public override void AppendAdditionalProperty(Appointment resource, CreateAppointmentCommand request)
	{
		if (request is null || resource is null)
		{
			return;
		}

		foreach (var treatmentId in request.TreatmentsIds)
		{
			var treatment = _context.Treatments.FirstOrDefault(t => t.Id == treatmentId);

			if (treatment == null)
			{
				continue;
			}

			resource.Treatments.Add(treatment);
		}
	}

	public override void AppendAdditionalValidation(CreateAppointmentCommand request)
	{
		var client = _context.Clients.FirstOrDefault(c => c.Id == request.ClientId);

		if (client is null)
		{
			throw new NotFoundException($"{NotFoundException.MessagePrefix} client");
		}
	}
}