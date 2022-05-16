using CosManagement.Entities;
using CosManagement.Mappings;

namespace CosManagement.Dtos.Treatments;

public class CreateTreatmentDto : BaseTreatmentDto, IMapFrom<Treatment>
{
	public Guid Id { get; set; }

	public Guid CategoryId { get; set; }
}