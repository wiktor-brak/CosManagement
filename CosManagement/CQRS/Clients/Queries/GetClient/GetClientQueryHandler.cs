using AutoMapper;
using CosManagement.Database;
using CosManagement.Dtos.Clients;
using CosManagement.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CosManagement.CQRS.Clients.Queries.GetClient;

public class GetClientQueryHandler : IRequestHandler<GetClientQuery, GetClientDto>
{
	private readonly ApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly ICurrentUserService _currentUserService;

	public GetClientQueryHandler(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
	{
		_context = context;
		_mapper = mapper;
		_currentUserService = currentUserService;
	}

	public async Task<GetClientDto> Handle(GetClientQuery request, CancellationToken cancellationToken)
	{
		return _mapper
			.Map<GetClientDto>(
				await _context.Clients
				.SingleOrDefaultAsync(c =>
					c.OwnerId == _currentUserService.UserId &&
					c.Id == request.Id, cancellationToken));
	}
}