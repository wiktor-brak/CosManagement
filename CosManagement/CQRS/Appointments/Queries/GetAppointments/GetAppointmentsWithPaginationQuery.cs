using CosManagement.Common.Models;
using CosManagement.Dtos.Appointments;
using CosManagement.Interfaces;
using MediatR;

namespace CosManagement.CQRS.Appointments.Queries.GetAppointments;

public class GetAppointmentsWithPaginationQuery : IRequest<PaginatedList<GetAppointmentDto>>, IGetAllQuery
{
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 10;
}