using AutoMapper;
using CosManagement.Database;
using CosManagement.Exceptions;
using CosManagement.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CosManagement.CQRS.Common.Queries;

public class GetBaseHandler<TQuery, TResponse, TResource> : IRequestHandler<TQuery, TResponse>
	where TQuery : IRequest<TResponse>, IIdParameter
	where TResource : class, IResource, IOwned

{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly ICurrentUserService _currentUserService;

	public GetBaseHandler(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
	{
		_context = context;
		_mapper = mapper;
		_currentUserService = currentUserService;
	}

	public async Task<TResponse> Handle(TQuery request, CancellationToken cancellationToken)
	{
		var resource = await _context.Set<TResource>()
			.SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

		if (resource is null)
		{
			throw new NotFoundException();
		}

		if (resource.OwnerId != _currentUserService.UserId)
		{
			throw new ForbiddenAccessException();
		}

		return _mapper.Map<TResponse>(resource);
	}
}