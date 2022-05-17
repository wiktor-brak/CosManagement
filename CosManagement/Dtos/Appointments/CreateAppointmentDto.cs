using CosManagement.Dtos.Treatments;
using CosManagement.Entities;
using CosManagement.Mappings;

namespace CosManagement.Dtos.Appointments;

public class CreateAppointmentDto : BaseAppointmentDto, IMapFrom<Appointment>
{
	public Guid Id { get; set; }

	public Guid ClientId { get; set; }

	public List<GetTreatmentDto> Treatments { get; set; } = new();
}