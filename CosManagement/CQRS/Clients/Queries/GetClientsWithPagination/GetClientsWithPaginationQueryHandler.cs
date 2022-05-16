using AutoMapper;
using CosManagement.CQRS.Common.Queries;
using CosManagement.Database;
using CosManagement.Dtos.Clients;
using CosManagement.Entities;
using CosManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CosManagement.CQRS.Clients.Queries.GetClientsWithPagination;

public class GetClientsWithPaginationQueryHandler : GetAllBaseHandler<GetClientsWithPaginationQuery, GetClientDto, Client>
{
	public GetClientsWithPaginationQueryHandler(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
		: base(context, mapper, currentUserService)
	{
	}

	public override IQueryable<Client> GetResource()
	{
		return base.GetResource()
			.Include(_ => _.Appointments)
			.ThenInclude(_ => _.Treatments);
	}
}