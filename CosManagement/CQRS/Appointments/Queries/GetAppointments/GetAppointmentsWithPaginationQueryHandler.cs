using AutoMapper;
using CosManagement.CQRS.Common.Queries;
using CosManagement.Database;
using CosManagement.Dtos.Appointments;
using CosManagement.Entities;
using CosManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CosManagement.CQRS.Appointments.Queries.GetAppointments;

public class GetAppointmentsWithPaginationQueryHandler : GetAllBaseHandler<GetAppointmentsWithPaginationQuery, GetAppointmentDto, Appointment>
{
	public GetAppointmentsWithPaginationQueryHandler(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
		: base(context, mapper, currentUserService)
	{
	}

	public override IQueryable<Appointment> GetResource()
	{
		return base.GetResource()
			.Include(_ => _.Client)
			.Include(_ => _.Treatments);
	}
}