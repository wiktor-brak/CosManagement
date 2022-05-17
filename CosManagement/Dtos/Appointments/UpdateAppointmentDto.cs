using CosManagement.Dtos.Treatments;
using CosManagement.Entities;
using CosManagement.Mappings;

namespace CosManagement.Dtos.Appointments;

public class UpdateAppointmentDto : BaseAppointmentDto, IMapFrom<Appointment>
{
	public Guid ClientId { get; set; }

	public List<GetTreatmentDto> Treatments { get; set; } = new();
}