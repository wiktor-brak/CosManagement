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

public class LoginEndpointTest : ApiBaseTest
{
	[Fact]
	public async Task PostLogin_WithCorrectData_ShouldReturnOkResponse()
	{
		var email = "TestUser@example.com";
		var password = "Password123!";

		await _httpClient.PostAsJsonAsync(ApiRoutes.Register, new RegisterModel
		{
			Email = email,
			Password = password,
			ConfirmPassoword = password
		});

		var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Login, new LoginModel
		{
			Username = email,
			Password = password
		});

		response.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Fact]
	public async Task PostLogin_WithIncorrectPassword_ShouldReturnOUnauthorizedResponse()
	{
		var email = "TestUser@example.com";
		var password = "Password";

		await _httpClient.PostAsJsonAsync(ApiRoutes.Register, new RegisterModel
		{
			Email = email,
			Password = password,
			ConfirmPassoword = password
		});

		var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Login, new LoginModel
		{
			Username = email,
			Password = password
		});

		response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
	}

	[Fact]
	public async Task PostRegister_WithInorrectData_ShouldReturnBadRequestResponse()
	{
		var email = "TestUser";
		var password = "password";

		var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Login, new RegisterModel
		{
			Email = email,
			Password = password,
		});

		response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}
}