using CosManagement.Dtos.Appointments;
using CosManagement.Interfaces;
using MediatR;

namespace CosManagement.CQRS.Appointments.Queries.GetAppointment;

public class GetAppointmentQuery : IRequest<GetAppointmentDto>, IGetQuery
{
	public Guid Id { get; set; }
}