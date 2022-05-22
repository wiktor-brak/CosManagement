using AutoMapper;
using CosManagement.Database;
using CosManagement.Exceptions;
using CosManagement.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CosManagement.CQRS.Common.Commands;

public abstract class UpdateBaseHandler<TCommand, TResource, TDto> : IRequestHandler<TCommand, Unit>
	where TCommand : IRequest<Unit>, IMultiSourceResponse<TDto>
	where TResource : class, IResource, IOwned
{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly ICurrentUserService _currentUserService;

	protected UpdateBaseHandler(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
	{
		_context = context;
		_mapper = mapper;
		_currentUserService = currentUserService;
	}

	public async Task<Unit> Handle(TCommand request, CancellationToken cancellationToken)
	{
		var dbEntry = await GetResources().FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

		if (dbEntry is null)
		{
			throw new NotFoundException();
		}

		if (dbEntry.OwnerId != _currentUserService.UserId)
		{
			throw new ForbiddenAccessException();
		}

		dbEntry = _mapper.Map(request.Dto, dbEntry);

		MapAdditionalProperties(dbEntry, request);

		_context.Set<TResource>().Update(dbEntry);

		await _context.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}

	// Override this method to include navigation properties
	public virtual IQueryable<TResource> GetResources()
	{
		return _context.Set<TResource>();
	}

	public virtual void MapAdditionalProperties(TResource resource, TCommand request)
	{ }
}