using CosManagement.Dtos.Clients;
using CosManagement.Dtos.Treatments;
using CosManagement.Entities;
using CosManagement.Mappings;

namespace CosManagement.Dtos.Appointments;

public class GetAppointmentDto : BaseAppointmentDto, IMapFrom<Appointment>
{
	public Guid Id { get; set; }
	public Guid ClientId { get; set; }

	public GetClientWithoutAppointmentsDto? Client { get; set; }

	public List<GetTreatmentDto> Treatments { get; set; } = new();
}