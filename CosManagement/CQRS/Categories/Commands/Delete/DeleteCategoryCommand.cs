using CosManagement.Interfaces;
using MediatR;

namespace CosManagement.CQRS.Categories.Commands.Delete;

public class DeleteCategoryCommand : IRequest<Unit>, IIdParameter
{
	public Guid Id { get; set; }
}