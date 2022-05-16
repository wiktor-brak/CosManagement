using CosManagement.Dtos.Categories;
using CosManagement.Interfaces;
using MediatR;

namespace CosManagement.CQRS.Categories.Queries.GetCategory;

public class GetCategoryQuery : IRequest<GetCategoryDto>, IIdParameter
{
	public Guid Id { get; set; }
}