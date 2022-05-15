using CosManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CosManagement.Database.Configurations;

public class CategoryConfig : IEntityTypeConfiguration<Category>
{
	public void Configure(EntityTypeBuilder<Category> builder)
	{
		builder.Property(c => c.Name)
			.HasMaxLength(500)
			.IsRequired();
	}
}