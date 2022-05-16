using AutoMapper;
using CosManagement.CQRS.Common.Queries;
using CosManagement.Database;
using CosManagement.Dtos.Appointments;
using CosManagement.Entities;
using CosManagement.Interfaces;

namespace CosManagement.CQRS.Appointments.Queries.GetAppointment;

public class GetAppointmentQueryHandler : GetBaseHandler<GetAppointmentQuery, GetAppointmentDto, Appointment>
{
	public GetAppointmentQueryHandler(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService) : base(context, mapper, currentUserService)
	{ }
}