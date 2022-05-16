using CosManagement.Entities;
using CosManagement.Mappings;

namespace CosManagement.Dtos.Clients;

public class GetClientWithoutAppointmentsDto : BaseClientDto, IMapFrom<Client>
{
}