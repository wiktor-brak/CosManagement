using CosManagement.Common;
using CosManagement.Dtos.Categories;
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

public class CategoriesEndpointsTest : ApiBaseTest, IAsyncLifetime
{
	private async Task<HttpResponseMessage?> CreateTestCategory(bool isValid, CreateCategoryDto? dto = default)
	{
		var category = dto;

		if (category is null)
		{
			if (isValid)
			{
				category = new CreateCategoryDto
				{
					Name = "TestName"
				};
			}
			else
			{
				category = new CreateCategoryDto();
			}
		}

		return await _httpClient.PostAsJsonAsync(ApiRoutes.Categories, category);
	}

	[Fact]
	public async Task GetAllCategories_ShouldReturnOkResponse()
	{
		var response = await _httpClient.GetAsync(ApiRoutes.Categories);

		response?.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task GetAllCategories_WithIncorrectQueryParameters_ShouldReturnBadRequestResponse()
	{
		var response = await _httpClient.GetAsync(ApiRoutes.Categories + "/?pageSize=100&pageNumber=-1");

		var errors = response?.Content.ReadFromJsonAsync<ValidationProblemDetails>()?.Result?.Errors.Values.SelectMany(s => s);

		errors.Should().Contain("PageNumber at least greater than or equal to 1.");
		errors.Should().Contain("PageSize max value is 50");
		response?.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task GetCategory_WithCorrectId_ShouldReturnOkResponseAndSameId()
	{
		var createResponse = await CreateTestCategory(true);
		var createdCategoryId = createResponse?.Content?.ReadFromJsonAsync<GetCategoryDto>()?.Result?.Id;

		var getResponse = await _httpClient.GetAsync(ApiRoutes.Category.Replace("{id}", createdCategoryId.ToString()));
		var categoryId = getResponse?.Content.ReadFromJsonAsync<GetCategoryDto>().Result?.Id;

		getResponse?.StatusCode.Should().Be(HttpStatusCode.OK);
		categoryId.Should().Be(createdCategoryId);
	}

	[Fact]
	public async Task GetCategory_WithIncorrectId_ShouldReturnNotFoundResponse()
	{
		var getResponse = await _httpClient.GetAsync(ApiRoutes.Category.Replace("{id}", Guid.NewGuid().ToString()));

		getResponse?.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task CreateCategory_WithCorrectData_ShouldReturnCreatedResponseAndCreatedCategory()
	{
		var response = await CreateTestCategory(true);

		var category = response?.Content.ReadFromJsonAsync<CreateCategoryDto>().Result;

		response?.StatusCode.Should().Be(HttpStatusCode.Created);
		category?.Name.Should().Be("TestName");
	}

	[Fact]
	public async Task CreateCategory_WithEmptyData_ShouldReturnBadRequestResponseAndContainErrors()
	{
		var response = await CreateTestCategory(false);

		var errors = response?.Content.ReadFromJsonAsync<ValidationProblemDetails>()?.Result?.Errors.Values.SelectMany(s => s);

		errors.Should().Contain("Name cannot be empty");
		response?.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task CreateCategory_WithIncorrectData_ShouldReturnBadRequestResponseAndContainErrors()
	{
		var response = await CreateTestCategory(false, new CreateCategoryDto
		{
			Name = string.Concat(Enumerable.Repeat("Test", 500)),
		});

		var errors = response?.Content.ReadFromJsonAsync<ValidationProblemDetails>()?.Result?.Errors.Values.SelectMany(s => s);

		errors.Should().Contain("Name length must not exceed 500.");
		response?.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task UpdateCategory_WithCorrectData_ShouldReturnOkResponse()
	{
		var createdResponse = await CreateTestCategory(true);
		var createdCategoryId = createdResponse?.Content?.ReadFromJsonAsync<CreateCategoryDto>()?.Result?.Id;

		var name = "TestName";

		var updateResponse = await _httpClient.PutAsJsonAsync(
			ApiRoutes.Category.Replace("{id}", createdCategoryId.ToString()),
			new CreateCategoryDto
			{
				Name = name
			});

		var getResponse = await _httpClient.GetAsync(ApiRoutes.Category.Replace("{id}", createdCategoryId.ToString()));
		var category = getResponse.Content.ReadFromJsonAsync<GetCategoryDto>().Result;

		updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
		category?.Name.Should().Be(name);
	}

	[Fact]
	public async Task UpdateCategory_WithIncorrectData_ShouldReturnBadRequestResponse()
	{
		var createdResponse = await CreateTestCategory(true);
		var createdCategoryId = createdResponse?.Content?.ReadFromJsonAsync<CreateCategoryDto>()?.Result?.Id;

		var response = await _httpClient.PutAsJsonAsync(ApiRoutes.Category.Replace("{id}", createdCategoryId.ToString()), new UpdateCategoryDto());

		response?.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task DeleteCategory_WithCorrectId_ShouldReturnNoContentResponse()
	{
		var createResponse = await CreateTestCategory(true);
		var createdCategoryId = createResponse?.Content?.ReadFromJsonAsync<GetCategoryDto>()?.Result?.Id;

		var deleteResponse = await _httpClient.DeleteAsync(ApiRoutes.Category.Replace("{id}", createdCategoryId.ToString()));

		deleteResponse?.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task DeleteCategory_WithIncorrectId_ShouldReturnNotFoundResponse()
	{
		var deleteResponse = await _httpClient.DeleteAsync(ApiRoutes.Category.Replace("{id}", Guid.NewGuid().ToString()));

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