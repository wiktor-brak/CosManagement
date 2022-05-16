using CosManagement.Dtos.Treatments;
using CosManagement.Entities;
using CosManagement.Mappings;

namespace CosManagement.Dtos.Appointments;

public class GetAppointmentWithoutClientDto : BaseAppointmentDto, IMapFrom<Appointment>
{
	public List<GetTreatmentDto> Treatments { get; set; } = new();
}