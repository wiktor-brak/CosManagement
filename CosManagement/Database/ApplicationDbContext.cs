using CosManagement.Entities;
using CosManagement.Interfaces;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace CosManagement.Database;

public class ApplicationDbContext : ApiAuthorizationDbContext<IdentityUser>
{
	private readonly ICurrentUserService _currentUserService;

	public ApplicationDbContext(
		DbContextOptions<ApplicationDbContext> options,
		IOptions<OperationalStoreOptions> operationalStoreOptions,
		ICurrentUserService currentUserService) : base(options, operationalStoreOptions)
	{
		_currentUserService = currentUserService;
	}

	public DbSet<Client> Clients => Set<Client>();

	public DbSet<Category> Categories => Set<Category>();

	public DbSet<Treatment> Treatments => Set<Treatment>();

	public DbSet<Appointment> Appointments => Set<Appointment>();

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
	{
		foreach (var entry in ChangeTracker.Entries<BaseEntity>())
		{
			switch (entry.State)
			{
				case EntityState.Added:
					entry.Entity.CreatedBy = _currentUserService.UserId;
					entry.Entity.Created = DateTime.Now;
					break;

				case EntityState.Modified:
					entry.Entity.LastModifiedBy = _currentUserService.UserId;
					entry.Entity.LastModified = DateTime.Now;
					break;
			}
		}

		var result = await base.SaveChangesAsync(cancellationToken);

		return result;
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		base.OnModelCreating(builder);
	}
}