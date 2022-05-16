using AutoMapper;
using AutoMapper.QueryableExtensions;
using CosManagement.Common.Models;
using CosManagement.Database;
using CosManagement.Dtos.Clients;
using CosManagement.Entities;
using CosManagement.Exceptions;
using CosManagement.Interfaces;
using CosManagement.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CosManagement.CQRS.Common.Queries;

public abstract class GetAllBaseHandler<TQuery, TResponse, TResource> : IRequestHandler<TQuery, PaginatedList<TResponse>>
	where TQuery : IRequest<PaginatedList<TResponse>>, IGetAllQuery
	where TResource : class, IResource, IOwned

{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly ICurrentUserService _currentUserService;

	protected GetAllBaseHandler(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
	{
		_context = context;
		_mapper = mapper;
		_currentUserService = currentUserService;
	}

	public async Task<PaginatedList<TResponse>> Handle(TQuery request, CancellationToken cancellationToken)
	{
		var resource = GetResource();

		if (resource is null)
		{
			throw new NotFoundException();
		}

		if (resource.FirstOrDefault()?.OwnerId != _currentUserService.UserId)
		{
			throw new UnauthorizedAccessException();
		}

		return await resource
				.ProjectTo<TResponse>(_mapper.ConfigurationProvider)
				.PaginatedListAsync(request.PageNumber, request.PageSize);
	}

	// Override this method to include additional properties
	public virtual IQueryable<TResource> GetResource() => _context.Set<TResource>().Where(c => c.OwnerId == _currentUserService.UserId);
}