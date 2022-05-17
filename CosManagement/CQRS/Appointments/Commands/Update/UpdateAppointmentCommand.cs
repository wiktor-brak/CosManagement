using CosManagement.Dtos.Appointments;
using CosManagement.Interfaces;
using MediatR;

namespace CosManagement.CQRS.Appointments.Commands.Update;

public class UpdateAppointmentCommand : IRequest<Unit>, IMultiSourceResponse<UpdateAppointmentDto>
{
	public Guid Id { get; set; }
	public UpdateAppointmentDto? Dto { get; set; }
}