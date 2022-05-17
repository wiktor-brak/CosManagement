using CosManagement.Dtos.Categories;
using CosManagement.Entities;
using CosManagement.Mappings;
using MediatR;

namespace CosManagement.CQRS.Categories.Commands.Create;

public class CreateCategoryCommand : IRequest<CreateCategoryDto>, IMapFrom<Category>
{
	public string? Name { get; set; }
}