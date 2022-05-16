using CosManagement.Entities;
using CosManagement.Mappings;

namespace CosManagement.Dtos.Treatments;

public class UpdateTreatmentDto : BaseTreatmentDto, IMapFrom<Treatment>
{
	public Guid CategoryId { get; set; }
}