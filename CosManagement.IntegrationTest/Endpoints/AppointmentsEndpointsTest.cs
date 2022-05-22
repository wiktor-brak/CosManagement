using CosManagement.Common;
using CosManagement.CQRS.Appointments.Commands.Create;
using CosManagement.Dtos.Appointments;
using CosManagement.Dtos.Clients;
using CosManagement.Dtos.Treatments;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace CosManagement.IntegrationTest;

public class AppointmentsEndpointsTest : ApiBaseTest, IAsyncLifetime
{
	private readonly DateTime _date = DateTime.Now;

	private async Task<HttpResponseMessage?> CreateTestAppointment(bool isValid, CreateAppointmentDto? dto = default)
	{
		var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Clients, new CreateClientDto { FirstName = "TestFirstName", LastName = "TestLastName" });
		var clientId = response?.Content?.ReadFromJsonAsync<GetClientDto>()?.Result?.Id;

		if (dto is not null)
		{
			return await _httpClient.PostAsJsonAsync(ApiRoutes.Appointments, dto);
		}

		return await _httpClient.PostAsJsonAsync(
			ApiRoutes.Appointments, isValid ?
			new CreateAppointmentDto
			{
				Date = _date,
				Note = "TestNote",
				ClientId = clientId!.Value,
			} : new CreateAppointmentDto());
	}

	[Fact]
	public async Task GetAllAppointments_ShouldReturnOkResponse()
	{
		var response = await _httpClient.GetAsync(ApiRoutes.Appointments);

		response?.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task GetAllAppointments_WithIncorrectQueryParameters_ShouldReturnBadRequestResponse()
	{
		var response = await _httpClient.GetAsync(ApiRoutes.Appointments + "/?pageSize=100&pageNumber=-1");

		var errors = response?.Content.ReadFromJsonAsync<ValidationProblemDetails>()?.Result?.Errors.Values.SelectMany(s => s);

		errors.Should().Contain("PageNumber at least greater than or equal to 1.");
		errors.Should().Contain("PageSize max value is 50");
		response?.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task GetAppointments_WithCorrectId_ShouldReturnOkResponseAndSameId()
	{
		var createResponse = await CreateTestAppointment(true);
		var createdAppointmentId = createResponse?.Content?.ReadFromJsonAsync<GetAppointmentDto>()?.Result?.Id;

		var getResponse = await _httpClient.GetAsync(ApiRoutes.Appointment.Replace("{id}", createdAppointmentId.ToString()));
		var appointmentId = getResponse?.Content.ReadFromJsonAsync<GetAppointmentDto>().Result?.Id;

		getResponse?.StatusCode.Should().Be(HttpStatusCode.OK);
		appointmentId.Should().Be(createdAppointmentId);
	}

	[Fact]
	public async Task GetAppointments_WithIncorrectId_ShouldReturnNotFoundResponse()
	{
		var getResponse = await _httpClient.GetAsync(ApiRoutes.Appointment.Replace("{id}", Guid.NewGuid().ToString()));

		getResponse?.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task CreateAppointments_WithCorrectData_ShouldReturnCreatedResponseAndCreatedAppointment()
	{
		var response = await CreateTestAppointment(true);

		var client = response?.Content.ReadFromJsonAsync<CreateAppointmentDto>().Result;

		response?.StatusCode.Should().Be(HttpStatusCode.Created);
		client?.Date.Should().Be(_date);
		client?.Note.Should().Be("TestNote");
	}

	[Fact]
	public async Task CreateAppointments_WithTreatmentsIds_ShouldReturnCreatedResponseAndCreatedAppointment()
	{
		var treatmentResponse = await _httpClient.PostAsJsonAsync(ApiRoutes.Treatments, new CreateTreatmentDto { Name = "TestName", BasePrice = 10M });
		var treatment = treatmentResponse?.Content?.ReadFromJsonAsync<GetTreatmentDto>()?.Result;

		var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Clients, new CreateClientDto { FirstName = "TestFirstName", LastName = "TestLastName" });
		var clientId = response?.Content?.ReadFromJsonAsync<GetClientDto>()?.Result?.Id;

		var command = new UpdateApptointmentCommand
		{
			Date = _date,
			Note = "Test Note",
			TreatmentsIds = new List<Guid> { treatment!.Id },
			ClientId = clientId!.Value
		};

		var appointmentResponse = await _httpClient.PostAsJsonAsync(ApiRoutes.Appointments, command);

		var client = appointmentResponse?.Content.ReadFromJsonAsync<CreateAppointmentDto>().Result;

		appointmentResponse?.StatusCode.Should().Be(HttpStatusCode.Created);
		client?.TreatmentsIds.Should().Contain(t => t == treatment.Id);
	}

	[Fact]
	public async Task CreateAppointment_WithEmptyData_ShouldReturnBadRequestResponseAndContainErrors()
	{
		var response = await CreateTestAppointment(false);

		var errors = response?.Content.ReadFromJsonAsync<ValidationProblemDetails>()?.Result?.Errors.Values.SelectMany(s => s);

		errors.Should().Contain("Appointment date cannot be empty");
		errors.Should().Contain("Client must be provided");
		response?.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task CreateAppointment_WithCorrectDataAndIncorrectClientId_ShouldReturnNotFoundResponse()
	{
		var response = await CreateTestAppointment(false, new CreateAppointmentDto
		{
			Note = "Test Name",
			Date = DateTime.Now,
			ClientId = Guid.NewGuid()
		});

		response?.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task UpdateAppointment_WithCorrectData_ShouldReturnOkResponse()
	{
		var createdResponse = await CreateTestAppointment(true);
		var createdAppointment = createdResponse?.Content?.ReadFromJsonAsync<CreateAppointmentDto>()?.Result;

		var clientId = createdAppointment?.ClientId;
		var date = DateTime.Now.AddDays(10);
		var note = "Updated note";

		var updateResponse = await _httpClient.PutAsJsonAsync(
			ApiRoutes.Appointment.Replace("{id}", createdAppointment?.Id.ToString()),
			new UpdateAppointmentDto
			{
				ClientId = clientId!.Value,
				Date = date,
				Note = note
			});

		var getResponse = await _httpClient.GetAsync(ApiRoutes.Appointment.Replace("{id}", createdAppointment?.Id.ToString()));
		var appointment = getResponse.Content.ReadFromJsonAsync<GetAppointmentDto>().Result;

		updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
		appointment?.Note.Should().Be(note);
		appointment?.Date.Should().Be(date);
		appointment?.ClientId.Should().Be(clientId.Value);
	}

	[Fact]
	public async Task UpdateAppointment_WithTreatmentsIds_ShouldReturnOkResponse()
	{
		var createdResponse = await CreateTestAppointment(true);
		var createdAppointment = createdResponse?.Content?.ReadFromJsonAsync<CreateAppointmentDto>()?.Result;

		var treatmentResponse = await _httpClient.PostAsJsonAsync(ApiRoutes.Treatments, new CreateTreatmentDto { Name = "TestName", BasePrice = 10M });
		var treatment = treatmentResponse?.Content?.ReadFromJsonAsync<GetTreatmentDto>()?.Result;

		var updateResponse = await _httpClient.PutAsJsonAsync(
			ApiRoutes.Appointment.Replace("{id}", createdAppointment?.Id.ToString()),
			new UpdateAppointmentDto
			{
				ClientId = createdAppointment.ClientId,
				Date = _date,
				Note = "Updated Note",
				TreatmentsIds = new List<Guid> { treatment!.Id }
			});

		updateResponse?.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task UpdateAppointment_WithIncorrectData_ShouldReturnBadRequestResponse()
	{
		var createdResponse = await CreateTestAppointment(true);
		var createdAppointmentId = createdResponse?.Content?.ReadFromJsonAsync<CreateAppointmentDto>()?.Result?.Id;

		var response = await _httpClient.PutAsJsonAsync(ApiRoutes.Appointment.Replace("{id}", createdAppointmentId.ToString()), new UpdateAppointmentDto());

		response?.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task DeleteAppointment_WithCorrectId_ShouldReturnNoContentResponse()
	{
		var createResponse = await CreateTestAppointment(true);
		var createdAppointmentId = createResponse?.Content?.ReadFromJsonAsync<GetAppointmentDto>()?.Result?.Id;

		var deleteResponse = await _httpClient.DeleteAsync(ApiRoutes.Appointment.Replace("{id}", createdAppointmentId.ToString()));

		deleteResponse?.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task DeleteAppointment_WithIncorrectId_ShouldReturnNotFoundResponse()
	{
		var deleteResponse = await _httpClient.DeleteAsync(ApiRoutes.Appointment.Replace("{id}", Guid.NewGuid().ToString()));

		deleteResponse?.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	public async Task InitializeAsync()
	{
		await AutenticateAsync();
	}

	Task IAsyncLifetime.DisposeAsync()
	{
		return Task.CompletedTask;
	}
}