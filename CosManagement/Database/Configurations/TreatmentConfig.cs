using CosManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosManagement.Database.Configurations;

public class TreatmentConfig : IEntityTypeConfiguration<Treatment>
{
	public void Configure(EntityTypeBuilder<Treatment> builder)
	{
		builder.Property(c => c.Name)
			.HasMaxLength(500)
			.IsRequired();

		builder.Property(c => c.BasePrice)
			.HasPrecision(10, 2)
			.IsRequired();

		builder.Property(c => c.CategoryId)
		   .IsRequired();
	}
}