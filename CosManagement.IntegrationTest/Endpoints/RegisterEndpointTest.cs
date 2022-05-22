using CosManagement.Common;
using CosManagement.Identity.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CosManagement.IntegrationTest.Endpoints;

public class RegisterEndpointTest : ApiBaseTest
{
	[Fact]
	public async Task PostRegister_WithCorrectData_ShouldReturnCreatedResponse()
	{
		var email = "TestUser@example.com";
		var password = "Password123!";

		var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Register, new RegisterModel
		{
			Email = email,
			Password = password,
			ConfirmPassoword = password
		});

		response.StatusCode.Should().Be(HttpStatusCode.Created);
	}

	[Fact]
	public async Task PostRegister_WithSameUsername_ShouldReturnBadRequestResponse()
	{
		var email = "TestUser@example.com";
		var password = "Password123!";

		await _httpClient.PostAsJsonAsync(ApiRoutes.Register, new RegisterModel
		{
			Email = email,
			Password = password,
			ConfirmPassoword = password
		});

		var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Register, new RegisterModel
		{
			Email = email,
			Password = password,
			ConfirmPassoword = password
		});

		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task PostRegister_WithInorrectData_ShouldReturnBadRequestResponse()
	{
		var email = "TestUser";
		var password = "password";

		var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Register, new RegisterModel
		{
			Email = email,
			Password = password,
		});

		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}
}