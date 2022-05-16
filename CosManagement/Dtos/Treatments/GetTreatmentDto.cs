using CosManagement.Entities;
using CosManagement.Mappings;

namespace CosManagement.Dtos.Treatments;

public class GetTreatmentDto : BaseTreatmentDto, IMapFrom<Treatment>
{
	public Guid Id { get; set; }
}