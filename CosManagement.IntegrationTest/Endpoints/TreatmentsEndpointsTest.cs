using CosManagement.Common;
using CosManagement.Dtos.Categories;
using CosManagement.Dtos.Treatments;
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

public class TreatmentsEndpointsTest : ApiBaseTest, IAsyncLifetime
{
	private async Task<HttpResponseMessage?> CreateTestTreatment(bool isValid, CreateTreatmentDto? dto = default)
	{
		var treatment = dto;

		if (treatment is null)
		{
			if (isValid)
			{
				treatment = new CreateTreatmentDto
				{
					Name = "TestName",
					BasePrice = 10m
				};
			}
			else
			{
				treatment = new CreateTreatmentDto();
			}
		}

		return await _httpClient.PostAsJsonAsync(ApiRoutes.Treatments, treatment);
	}

	[Fact]
	public async Task GetAllTreatments_ShouldReturnOkResponse()
	{
		var response = await _httpClient.GetAsync(ApiRoutes.Treatments);

		response?.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task GetAllTreatments_WithIncorrectQueryParameters_ShouldReturnBadRequestResponse()
	{
		var response = await _httpClient.GetAsync(ApiRoutes.Treatments + "/?pageSize=100&pageNumber=-1");

		var errors = response?.Content.ReadFromJsonAsync<ValidationProblemDetails>()?.Result?.Errors.Values.SelectMany(s => s);

		errors.Should().Contain("PageNumber at least greater than or equal to 1.");
		errors.Should().Contain("PageSize max value is 50");
		response?.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task GetTreatment_WithCorrectId_ShouldReturnOkResponseAndSameId()
	{
		var createResponse = await CreateTestTreatment(true);
		var createdTreatmentId = createResponse?.Content?.ReadFromJsonAsync<GetTreatmentDto>()?.Result?.Id;

		var getResponse = await _httpClient.GetAsync(ApiRoutes.Treatment.Replace("{id}", createdTreatmentId.ToString()));
		var treatmentId = getResponse?.Content.ReadFromJsonAsync<GetTreatmentDto>().Result?.Id;

		getResponse?.StatusCode.Should().Be(HttpStatusCode.OK);
		treatmentId.Should().Be(createdTreatmentId);
	}

	[Fact]
	public async Task GetTreatment_WithIncorrectId_ShouldReturnNotFoundResponse()
	{
		var getResponse = await _httpClient.GetAsync(ApiRoutes.Treatment.Replace("{id}", Guid.NewGuid().ToString()));

		getResponse?.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task CreateTreatment_WithCorrectData_ShouldReturnCreatedResponseAndCreatedTreatment()
	{
		var response = await CreateTestTreatment(true);

		var client = response?.Content.ReadFromJsonAsync<CreateTreatmentDto>().Result;

		response?.StatusCode.Should().Be(HttpStatusCode.Created);
		client?.Name.Should().Be("TestName");
		client?.BasePrice.Should().Be(10M);
	}

	[Fact]
	public async Task CreateTreatment_WithCategory_ShouldReturnCreatedResponseAndCreatedTreatment()
	{
		var createdCategoryResponse = await _httpClient.PostAsJsonAsync(ApiRoutes.Categories, new CreateCategoryDto { Name = "TestCategory" });
		var createdCategoryId = createdCategoryResponse.Content.ReadFromJsonAsync<CreateCategoryDto>()?.Result?.Id;
		var response = await CreateTestTreatment(true, new CreateTreatmentDto
		{
			Name = "TestName",
			BasePrice = 10M,
			CategoryId = createdCategoryId!.Value
		});

		var treatment = response?.Content.ReadFromJsonAsync<CreateTreatmentDto>().Result;

		response?.StatusCode.Should().Be(HttpStatusCode.Created);
		treatment?.Name.Should().Be("TestName");
		treatment?.CategoryId.Should().Be(createdCategoryId!.Value);
		treatment?.BasePrice.Should().Be(10M);
	}

	[Fact]
	public async Task CreateTreatment_WithEmptyData_ShouldReturnBadRequestResponseAndContainErrors()
	{
		var response = await CreateTestTreatment(false);

		var errors = response?.Content.ReadFromJsonAsync<ValidationProblemDetails>()?.Result?.Errors.Values.SelectMany(s => s);

		errors.Should().Contain("Name cannot be empty");
		errors.Should().Contain("Price must be greater than 0");
		response?.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task CreateTreatment_WithIncorrectData_ShouldReturnBadRequestResponseAndContainErrors()
	{
		var response = await CreateTestTreatment(false, new CreateTreatmentDto
		{
			Name = string.Concat(Enumerable.Repeat("Test", 500)),
			BasePrice = -1M,
			CategoryId = Guid.NewGuid()
		});

		var errors = response?.Content.ReadFromJsonAsync<ValidationProblemDetails>()?.Result?.Errors.Values.SelectMany(s => s);

		errors.Should().Contain("Name length must not exceed 500.");
		errors.Should().Contain("Price must be greater than 0");
		response?.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task CreateTreatment_WithCorrectDataAndIncorrectCategoryId_ShouldReturnNotFoundResponseAndContainErrors()
	{
		var response = await CreateTestTreatment(false, new CreateTreatmentDto
		{
			Name = "Test Name",
			BasePrice = 10M,
			CategoryId = Guid.NewGuid()
		});

		response?.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task UpdateTreatment_WithCorrectData_ShouldReturnOkResponse()
	{
		var createdResponse = await CreateTestTreatment(true);
		var createdTreatmentId = createdResponse?.Content?.ReadFromJsonAsync<CreateTreatmentDto>()?.Result?.Id;

		var name = "UpdatedTestName";
		var basePrice = 50M;

		var updateResponse = await _httpClient.PutAsJsonAsync(
			ApiRoutes.Treatment.Replace("{id}", createdTreatmentId.ToString()),
			new UpdateTreatmentDto
			{
				Name = name,
				BasePrice = basePrice
			});

		var getResponse = await _httpClient.GetAsync(ApiRoutes.Treatment.Replace("{id}", createdTreatmentId.ToString()));
		var treatment = getResponse.Content.ReadFromJsonAsync<GetTreatmentDto>().Result;

		updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
		treatment?.Name.Should().Be(name);
		treatment?.BasePrice.Should().Be(basePrice);
	}

	[Fact]
	public async Task UpdateTreatment_WithIncorrectData_ShouldReturnBadRequestResponse()
	{
		var createdResponse = await CreateTestTreatment(true);
		var createdTreatmentId = createdResponse?.Content?.ReadFromJsonAsync<CreateTreatmentDto>()?.Result?.Id;

		var response = await _httpClient.PutAsJsonAsync(ApiRoutes.Treatment.Replace("{id}", createdTreatmentId.ToString()), new UpdateTreatmentDto());

		response?.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task DeleteTreatment_WithCorrectId_ShouldReturnNoContentResponse()
	{
		var createResponse = await CreateTestTreatment(true);
		var createdTreatmentId = createResponse?.Content?.ReadFromJsonAsync<GetTreatmentDto>()?.Result?.Id;

		var deleteResponse = await _httpClient.DeleteAsync(ApiRoutes.Treatment.Replace("{id}", createdTreatmentId.ToString()));

		deleteResponse?.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task DeleteTreatment_WithIncorrectId_ShouldReturnNotFoundResponse()
	{
		var deleteResponse = await _httpClient.DeleteAsync(ApiRoutes.Treatment.Replace("{id}", Guid.NewGuid().ToString()));

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