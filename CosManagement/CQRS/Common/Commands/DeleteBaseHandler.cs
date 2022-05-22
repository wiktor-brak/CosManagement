using CosManagement.Database;
using CosManagement.Exceptions;
using CosManagement.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CosManagement.CQRS.Common.Commands;

public class DeleteBaseHandler<TCommand, TResource> : IRequestHandler<TCommand, Unit>
	where TCommand : IRequest<Unit>, IIdParameter
	where TResource : class, IResource, IOwned
{
	private readonly ApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;

	public DeleteBaseHandler(
		ApplicationDbContext context,
		ICurrentUserService currentUserService)
	{
		_context = context;
		_currentUserService = currentUserService;
	}

	public async Task<Unit> Handle(TCommand request, CancellationToken cancellationToken)
	{
		var entry = await _context.Set<TResource>().FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

		if (entry is null)
		{
			throw new NotFoundException();
		}

		if (entry.OwnerId != _currentUserService.UserId)
		{
			throw new ForbiddenAccessException();
		}

		_context.Set<TResource>().Remove(entry);
		await _context.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}