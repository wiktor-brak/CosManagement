using CosManagement.Common.Models;
using CosManagement.Dtos.Treatments;
using CosManagement.Interfaces;
using MediatR;

namespace CosManagement.CQRS.Treatments.Queries.GetTreatmentWithPagination;

public class GetTreatmentsWithPaginationQuery : IRequest<PaginatedList<GetTreatmentDto>>, IGetAllQuery
{
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 10;
}