using CosManagement.Dtos.Treatments;
using CosManagement.Interfaces;
using MediatR;

namespace CosManagement.CQRS.Treatments.Commands.Update;

public class UpdateTreatmentCommand : IRequest<Unit>, IMultiSourceResponse<UpdateTreatmentDto>
{
	public Guid Id { get; set; }
	public UpdateTreatmentDto? Dto { get; set; }
}