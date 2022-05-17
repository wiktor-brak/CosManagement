using CosManagement.Dtos.Categories;
using CosManagement.Interfaces;
using MediatR;

namespace CosManagement.CQRS.Categories.Commands.Update;

public class UpdateCategoryCommand : IRequest<Unit>, IMultiSourceResponse<UpdateCategoryDto>
{
	public Guid Id { get; set; }
	public UpdateCategoryDto? Dto { get; set; }
}