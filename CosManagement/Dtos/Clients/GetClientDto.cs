using CosManagement.Entities;
using CosManagement.Mappings;

namespace CosManagement.Dtos.Clients;

public class GetClientDto : BaseClientDto, IMapFrom<Client>
{
	public List<Appointment> Appointments { get; set; } = new();
}