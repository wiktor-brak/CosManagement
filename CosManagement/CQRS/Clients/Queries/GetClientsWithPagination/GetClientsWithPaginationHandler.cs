using AutoMapper;
using AutoMapper.QueryableExtensions;
using CosManagement.Common.Models;
using CosManagement.Database;
using CosManagement.Dtos.Clients;
using CosManagement.Interfaces;
using CosManagement.Mappings;
using MediatR;

using Microsoft.EntityFrameworkCore;

namespace CosManagement.CQRS.Clients.Queries.GetClientsWithPagination;

public class GetClientsWithPaginationQueryHandler : IRequestHandler<GetClientsWithPaginationQuery, PaginatedList<GetClientDto>>
{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly ICurrentUserService _currentUserService;

	public GetClientsWithPaginationQueryHandler(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
	{
		_context = context;
		_mapper = mapper;
		_currentUserService = currentUserService;
	}

	public async Task<PaginatedList<GetClientDto>> Handle(GetClientsWithPaginationQuery request, CancellationToken cancellationToken)
	{
		return await _context.Clients
			.Where(c => c.OwnerId == _currentUserService.UserId)
			.Include(_ => _.Appointments)
			.ThenInclude(_ => _.Treatments)
			.ThenInclude(_ => _.Category)
			.ProjectTo<GetClientDto>(_mapper.ConfigurationProvider)
			.PaginatedListAsync(request.PageNumber, request.PageSize);
	}
}