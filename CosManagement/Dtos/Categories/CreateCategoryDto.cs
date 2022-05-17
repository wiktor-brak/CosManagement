using CosManagement.Entities;
using CosManagement.Mappings;

namespace CosManagement.Dtos.Categories;

public class CreateCategoryDto : BaseCategoryDto, IMapFrom<Category>
{
	public Guid Id { get; set; }
}