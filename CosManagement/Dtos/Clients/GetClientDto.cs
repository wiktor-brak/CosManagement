using CosManagement.Dtos.Appointments;
using CosManagement.Entities;
using CosManagement.Mappings;

namespace CosManagement.Dtos.Clients;

public class GetClientDto : BaseClientDto, IMapFrom<Client>
{
	public List<GetAppointmentWithoutClientDto> Appointments { get; set; } = new();
}