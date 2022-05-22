using AutoMapper;
using CosManagement.Dtos.Appointments;
using CosManagement.Dtos.Categories;
using CosManagement.Dtos.Clients;
using CosManagement.Dtos.Treatments;
using CosManagement.Entities;
using CosManagement.Mappings;
using FluentAssertions;
using System;
using System.Runtime.Serialization;
using Xunit;

namespace CosManagement.UnitTest.Mappings;

public class MappingTests
{
	private readonly IConfigurationProvider _configuration;
	private readonly IMapper _mapper;

	public MappingTests()
	{
		_configuration = new MapperConfiguration(config =>
		{
			config.AddProfile<MappingProfile>();
			config.AddProfile<AdditionalMappings>();
		});

		_mapper = _configuration.CreateMapper();
	}

	[Fact]
	public void Mapper_ShouldHaveValidConfiguration()
	{
		_configuration.AssertConfigurationIsValid();
	}

	[Theory]
	[InlineData(typeof(Client), typeof(GetClientDto))]
	[InlineData(typeof(Client), typeof(CreateClientDto))]
	[InlineData(typeof(Client), typeof(GetClientWithoutAppointmentsDto))]
	[InlineData(typeof(Client), typeof(UpdateClientDto))]
	[InlineData(typeof(Appointment), typeof(GetAppointmentDto))]
	[InlineData(typeof(Appointment), typeof(GetAppointmentWithoutClientDto))]
	[InlineData(typeof(Appointment), typeof(CreateAppointmentDto))]
	[InlineData(typeof(Appointment), typeof(UpdateAppointmentDto))]
	[InlineData(typeof(Category), typeof(GetCategoryDto))]
	[InlineData(typeof(Category), typeof(CreateCategoryDto))]
	[InlineData(typeof(Category), typeof(UpdateCategoryDto))]
	[InlineData(typeof(Treatment), typeof(GetTreatmentDto))]
	[InlineData(typeof(Treatment), typeof(CreateTreatmentDto))]
	[InlineData(typeof(Treatment), typeof(UpdateTreatmentDto))]
	public void Mapper_CorrectMappings_ShoudlSupportMappingFromSourceToDestination(Type source, Type destination)
	{
		var instance = GetInstanceOf(source);

		var mappedObject = _mapper.Map(instance, source, destination);

		mappedObject.GetType().Should().Be(destination);
	}

	private static object GetInstanceOf(Type type)
	{
		if (type.GetConstructor(Type.EmptyTypes) != null)
			return Activator.CreateInstance(type)!;

		return FormatterServices.GetUninitializedObject(type);
	}
}