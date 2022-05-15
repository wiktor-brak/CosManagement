using CosManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosManagement.Database.Configurations;

public class AppointmentConfig : IEntityTypeConfiguration<Appointment>
{
	public void Configure(EntityTypeBuilder<Appointment> builder)
	{
		builder.Property(c => c.Date)
			.IsRequired();

		builder.Property(c => c.ClientId)
			.IsRequired();
	}
}