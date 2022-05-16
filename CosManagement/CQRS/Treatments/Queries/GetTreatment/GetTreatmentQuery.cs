using CosManagement.Dtos.Treatments;
using CosManagement.Interfaces;
using MediatR;

namespace CosManagement.CQRS.Treatments.Queries;

public class GetTreatmentQuery : IRequest<GetTreatmentDto>, IIdParameter
{
	public Guid Id { get; set; }
}