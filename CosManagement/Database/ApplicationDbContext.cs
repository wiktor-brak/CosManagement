using CosManagement.Entities;
using CosManagement.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CosManagement.Database;

public class ApplicationDbContext : DbContext
{
	private readonly ICurrentUserService _currentUserService;

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService) : base(options)
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

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		base.OnModelCreating(modelBuilder);
	}
}