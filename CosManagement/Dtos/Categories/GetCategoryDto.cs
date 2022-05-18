using CosManagement.Entities;
using CosManagement.Mappings;

namespace CosManagement.Dtos.Categories;

public class GetCategoryDto : BaseCategoryDto, IMapFrom<Category>
{
	public Guid Id { get; set; }
}