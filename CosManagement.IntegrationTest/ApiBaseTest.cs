using CosManagement.Identity.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace CosManagement.IntegrationTest;

public class ApiBaseTest : CustomWebApplicationFactory<Program>
{
	protected readonly HttpClient _httpClient;

	protected ApiBaseTest() : this(new CustomWebApplicationFactory<Program>())
	{ }

	protected ApiBaseTest(CustomWebApplicationFactory<Program> factory)
	{
		_httpClient = factory.CreateClient();
	}

	protected async Task AutenticateAsync()
	{
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetJwtAsync());
	}

	private async Task<string> GetJwtAsync()
	{
		var email = "TestUser@example.com";
		var password = "Password123!";

		await _httpClient.PostAsJsonAsync("/api/register", new RegisterModel
		{
			Email = email,
			Password = password,
			ConfirmPassoword = password
		});

		var response = await _httpClient.PostAsJsonAsync("/api/login", new LoginModel
		{
			Username = email,
			Password = password
		});

		var token = response.Content.ReadFromJsonAsync<JwtResponse>()?.Result?.Token;

		return token!;
	}
}