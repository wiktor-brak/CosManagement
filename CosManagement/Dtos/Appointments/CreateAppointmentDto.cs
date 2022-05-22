using CosManagement.Dtos.Treatments;
using CosManagement.Entities;
using CosManagement.Mappings;

namespace CosManagement.Dtos.Appointments;

public class CreateAppointmentDto : BaseAppointmentDto
{
	public Guid Id { get; set; }

	public Guid ClientId { get; set; }

	public List<Guid> TreatmentsIds { get; set; } = new();
}