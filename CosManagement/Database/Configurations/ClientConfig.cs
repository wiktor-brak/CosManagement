using CosManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosManagement.Database.Configurations;

public class ClientConfig : IEntityTypeConfiguration<Client>
{
	public void Configure(EntityTypeBuilder<Client> builder)
	{
		builder.Property(c => c.FirstName)
			.HasMaxLength(200)
			.IsRequired();

		builder.Property(c => c.LastName)
			.HasMaxLength(200)
			.IsRequired();

		builder.Property(c => c.Phone)
			.HasMaxLength(12)
			.IsRequired();
	}
}