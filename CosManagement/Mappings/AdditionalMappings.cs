using AutoMapper;
using CosManagement.CQRS.Appointments.Commands.Create;
using CosManagement.Dtos.Appointments;
using CosManagement.Entities;

namespace CosManagement.Mappings;

public class AdditionalMappings : Profile
{
	public AdditionalMappings()
	{
		CreateMap<Appointment, UpdateApptointmentCommand>()
			.ForMember(a => a.TreatmentsIds,
				opt => opt.MapFrom(a => a.Treatments.Select(t => t.Id)))
			.ReverseMap();

		CreateMap<Appointment, CreateAppointmentDto>()
			.ForMember(a => a.TreatmentsIds,
				opt => opt.MapFrom(a => a.Treatments.Select(t => t.Id)))
			.ReverseMap();

		CreateMap<Appointment, UpdateAppointmentDto>()
			.ForMember(a => a.TreatmentsIds,
				opt => opt.MapFrom(a => a.Treatments.Select(t => t.Id)))
			.ReverseMap();
	}
}