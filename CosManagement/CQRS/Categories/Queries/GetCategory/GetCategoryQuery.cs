using CosManagement.Dtos.Categories;
using CosManagement.Interfaces;
using MediatR;

namespace CosManagement.CQRS.Categories.Queries.GetCategory;

public class GetCategoryQuery : IRequest<GetCategoryDto>, IGetQuery
{
	public Guid Id { get; set; }
}