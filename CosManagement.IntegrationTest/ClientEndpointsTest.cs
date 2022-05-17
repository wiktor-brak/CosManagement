﻿using CosManagement.Common;
using CosManagement.CQRS.Clients.Commands.Create;
using CosManagement.CQRS.Clients.Commands.Update;
using CosManagement.Dtos.Clients;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace CosManagement.IntegrationTest;

public class ClientEndpointsTest : ApiBaseTest, IAsyncLifetime
{
	private async Task<HttpResponseMessage?> CreateTestClient(bool isValid, CreateClientDto? command = default)
	{
		if (isValid)
		{
			return await _httpClient.PostAsJsonAsync(ApiRoutes.Clients, new CreateClientDto
			{
				FirstName = "TestFirstName",
				LastName = "TestLastName",
				Email = "test@email.com",
				AdditionalInformations = "TestInfo",
				Phone = "111222333"
			});
		}

		if (command != null)
		{
			return await _httpClient.PostAsJsonAsync(ApiRoutes.Clients, command);
		}

		return await _httpClient.PostAsJsonAsync(ApiRoutes.Clients, new CreateClientDto());
	}

	[Fact]
	public async Task Get_WithCorrectId_ShouldReturnOkResponseAndSameId()
	{
		var createResponse = await CreateTestClient(true);
		var createdClientId = createResponse?.Content?.ReadFromJsonAsync<GetClientDto>()?.Result?.Id;

		var getResponse = await _httpClient.GetAsync(ApiRoutes.Client.Replace("{id}", createdClientId.ToString()));
		var clientId = getResponse?.Content.ReadFromJsonAsync<GetClientDto>().Result?.Id;

		getResponse?.StatusCode.Should().Be(HttpStatusCode.OK);
		clientId.Should().Be(createdClientId);
	}

	[Fact]
	public async Task Get_WithIncorrectId_ShouldReturnNotFoundResponse()
	{
		var getResponse = await _httpClient.GetAsync(ApiRoutes.Client.Replace("{id}", Guid.NewGuid().ToString()));

		getResponse?.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Create_WithCorrectData_ShouldReturnCreatedResponseAndCreatedClient()
	{
		var response = await CreateTestClient(true);

		var client = response?.Content.ReadFromJsonAsync<CreateClientDto>().Result;

		response?.StatusCode.Should().Be(HttpStatusCode.Created);
		client?.FirstName.Should().Be("TestFirstName");
		client?.LastName.Should().Be("TestLastName");
		client?.Email.Should().Be("test@email.com");
		client?.AdditionalInformations.Should().Be("TestInfo");
		client?.Phone.Should().Be("111222333");
	}

	[Fact]
	public async Task Create_WithEmptyData_ShouldReturnBadRequestResponseAndContainErrors()
	{
		var response = await CreateTestClient(false);

		var errors = response?.Content.ReadFromJsonAsync<ValidationProblemDetails>()?.Result?.Errors.Values.Select(s => string.Join("", s));

		errors.Should().Contain("First name cannot be empty");
		errors.Should().Contain("Last name cannot be empty");
		response?.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Create_WithIncorrectData_ShouldReturnBadRequestResponseAndContainErrors()
	{
		var response = await CreateTestClient(false, new CreateClientDto
		{
			Email = "test",
			Phone = "111222333124124214124124124"
		});

		var errors = response?.Content.ReadFromJsonAsync<ValidationProblemDetails>()?.Result?.Errors.Values.Select(s => string.Join("", s));

		errors.Should().Contain("First name cannot be empty");
		errors.Should().Contain("Last name cannot be empty");
		errors.Should().Contain("Provide correct email address");
		errors.Should().Contain("Phone number length must not exceed 12.");
		response?.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Update_WithCorrectData_ShouldReturnOkResponse()
	{
		var createdResponse = await CreateTestClient(true);
		var createdClientId = createdResponse?.Content?.ReadFromJsonAsync<CreateClientDto>()?.Result?.Id;

		var firstName = "UpdatedTestFirstName";
		var lastName = "UpdatedTestLastName";
		var email = "Updatedtest@email.com";
		var additionalInformations = "UpdatedTestInfo";
		var Phone = "333222111";

		var updateResponse = await _httpClient.PutAsJsonAsync(
			ApiRoutes.Client.Replace("{id}", createdClientId.ToString()),
			new UpdateClientDto
			{
				FirstName = firstName,
				LastName = lastName,
				Email = email,
				AdditionalInformations = additionalInformations,
				Phone = Phone
			});

		var getResponse = await _httpClient.GetAsync(ApiRoutes.Client.Replace("{id}", createdClientId.ToString()));
		var client = getResponse?.Content.ReadFromJsonAsync<GetClientDto>().Result;

		updateResponse?.StatusCode.Should().Be(HttpStatusCode.OK);
		client?.FirstName.Should().Be(firstName);
		client?.LastName.Should().Be(lastName);
		client?.Email.Should().Be(email);
		client?.AdditionalInformations.Should().Be(additionalInformations);
		client?.Phone.Should().Be(Phone);
	}

	[Fact]
	public async Task Update_WithIncorrectData_ShouldReturnBadRequestResponse()
	{
		var createdResponse = await CreateTestClient(true);
		var createdClientId = createdResponse?.Content?.ReadFromJsonAsync<CreateClientDto>()?.Result?.Id;

		var response = await _httpClient.PutAsJsonAsync(ApiRoutes.Client.Replace("{id}", createdClientId.ToString()), new UpdateClientDto());

		response?.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task Delete_WithCorrectId_ShouldReturnNoContentResponse()
	{
		var createResponse = await CreateTestClient(true);
		var createdClientId = createResponse?.Content?.ReadFromJsonAsync<GetClientDto>()?.Result?.Id;

		var deleteResponse = await _httpClient.DeleteAsync(ApiRoutes.Client.Replace("{id}", createdClientId.ToString()));

		deleteResponse?.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task Delete_WithIncorrectId_ShouldReturnNotFoundResponse()
	{
		var deleteResponse = await _httpClient.DeleteAsync(ApiRoutes.Client.Replace("{id}", Guid.NewGuid().ToString()));

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