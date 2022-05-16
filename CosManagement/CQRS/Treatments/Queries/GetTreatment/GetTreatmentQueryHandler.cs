using AutoMapper;
using CosManagement.CQRS.Common.Queries;
using CosManagement.Database;
using CosManagement.Dtos.Treatments;
using CosManagement.Entities;
using CosManagement.Interfaces;

namespace CosManagement.CQRS.Treatments.Queries;

public class GetTreatmentQueryHandler : GetBaseHandler<GetTreatmentQuery, GetTreatmentDto, Treatment>
{
	public GetTreatmentQueryHandler(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService) : base(context, mapper, currentUserService)
	{ }
}