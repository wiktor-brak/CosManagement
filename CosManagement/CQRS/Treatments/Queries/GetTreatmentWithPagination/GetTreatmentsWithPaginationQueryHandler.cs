using AutoMapper;
using CosManagement.CQRS.Common.Queries;
using CosManagement.Database;
using CosManagement.Dtos.Treatments;
using CosManagement.Entities;
using CosManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CosManagement.CQRS.Treatments.Queries.GetTreatmentWithPagination;

public class GetTreatmentsWithPaginationQueryHandler : GetAllBaseHandler<GetTreatmentsWithPaginationQuery, GetTreatmentDto, Treatment>
{
	public GetTreatmentsWithPaginationQueryHandler(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
		: base(context, mapper, currentUserService)
	{
	}

	public override IQueryable<Treatment> GetResource()
	{
		return base.GetResource()
			.Include(_ => _.Category);
	}
}