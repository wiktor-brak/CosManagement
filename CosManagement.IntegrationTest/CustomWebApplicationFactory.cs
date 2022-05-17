using CosManagement.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace CosManagement.IntegrationTest;

public class CustomWebApplicationFactory<TStartup>
	: WebApplicationFactory<TStartup> where TStartup : class
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureServices(services =>
		{
			var descriptor = services.SingleOrDefault(
				d => d.ServiceType ==
					typeof(DbContextOptions<ApplicationDbContext>));

			services.Remove(descriptor!);

			services.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseInMemoryDatabase("InMemoryDbForTesting");
			});

			var sp = services.BuildServiceProvider();

			using (var scope = sp.CreateScope())
			{
				var scopedServices = scope.ServiceProvider;
				var db = scopedServices.GetRequiredService<ApplicationDbContext>();

				db.Database.EnsureCreated();
			}
		});
	}
}