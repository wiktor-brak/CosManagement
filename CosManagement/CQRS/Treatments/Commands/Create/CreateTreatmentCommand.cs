using CosManagement.Dtos.Treatments;
using CosManagement.Entities;
using CosManagement.Mappings;
using MediatR;

namespace CosManagement.CQRS.Treatments.Commands.Create;

public class CreateTreatmentCommand : IRequest<CreateTreatmentDto>, IMapFrom<Treatment>
{
	public string? Name { get; set; }
	public decimal BasePrice { get; set; }

	public Guid CategoryId { get; set; }
}