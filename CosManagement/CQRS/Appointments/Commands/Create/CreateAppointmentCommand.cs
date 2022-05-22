using CosManagement.Dtos.Appointments;
using CosManagement.Entities;
using CosManagement.Mappings;
using MediatR;

namespace CosManagement.CQRS.Appointments.Commands.Create;

public class UpdateApptointmentCommand : IRequest<CreateAppointmentDto>
{
	public DateTime Date { get; set; }
	public string? Note { get; set; }

	public Guid ClientId { get; set; }
	public List<Guid> TreatmentsIds { get; set; } = new();
}