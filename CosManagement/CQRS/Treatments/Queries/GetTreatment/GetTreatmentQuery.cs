using CosManagement.Dtos.Treatments;
using CosManagement.Interfaces;
using MediatR;

namespace CosManagement.CQRS.Treatments.Queries;

public class GetTreatmentQuery : IRequest<GetTreatmentDto>, IGetQuery
{
	public Guid Id { get; set; }
}