using CosManagement.Common;
using CosManagement.Dtos.Clients;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CosManagement.IntegrationTest.Security;

public class AuthorizationTest : ApiBaseTest
{
	public record JsonRequest();

	[Fact]
	public async Task Enpoints_WithoutAuthorization_ShouldReturnUnauthorizedResponse()
	{
		foreach (var route in typeof(ApiRoutes).GetProperties())
		{
			if (route.Name == "Base" || route.Name == "Login" || route.Name == "Register")
			{
				continue;
			}

			var value = route.GetValue(null)?.ToString();

			if (value is not null && value.EndsWith('s'))
			{
				var getResponse = await _httpClient.GetAsync(value);
				var createResponse = await _httpClient.PostAsJsonAsync<JsonRequest>(value, null!);

				getResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
				createResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
			}

			if (value is not null && !value.EndsWith('s'))
			{
				var updateResponse = await _httpClient.PutAsJsonAsync<JsonRequest>(value.Replace("{id}", Guid.NewGuid().ToString()), null!);
				var deleteResponse = await _httpClient.DeleteAsync(value.Replace("{id}", Guid.NewGuid().ToString()));

				updateResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
				deleteResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
			}
		}
	}
}