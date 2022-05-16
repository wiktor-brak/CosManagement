using AutoMapper;
using CosManagement.CQRS.Common.Queries;
using CosManagement.Database;
using CosManagement.Dtos.Clients;
using CosManagement.Entities;
using CosManagement.Interfaces;

namespace CosManagement.CQRS.Clients.Queries.GetClient;

public class GetClientQueryHandler : GetBaseHandler<GetClientQuery, GetClientDto, Client>
{
	public GetClientQueryHandler(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService) : base(context, mapper, currentUserService)
	{ }
}