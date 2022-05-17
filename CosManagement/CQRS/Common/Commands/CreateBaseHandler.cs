using AutoMapper;
using CosManagement.Database;
using CosManagement.Interfaces;
using MediatR;

namespace CosManagement.CQRS.Common;

public abstract class CreateBaseHandler<TCommand, TResponse, TResource> : IRequestHandler<TCommand, TResponse>
	where TCommand : IRequest<TResponse>
	where TResource : class, IResource, IOwned
{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly ICurrentUserService _currentUserService;

	protected CreateBaseHandler(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
	{
		_context = context;
		_mapper = mapper;
		_currentUserService = currentUserService;
	}

	public async Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken)
	{
		var entry = _mapper.Map<TResource>(request);
		entry.OwnerId = _currentUserService.UserId;

		AppendAdditionalProperty(entry, request);
		AppendAdditionalValidation(request);

		await _context.Set<TResource>().AddAsync(entry, cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);

		return _mapper.Map<TResponse>(entry);
	}

	// Method to override if additional property is needed
	public virtual void AppendAdditionalProperty(TResource resource, TCommand request)
	{ }

	public virtual void AppendAdditionalValidation(TCommand request)
	{ }
}