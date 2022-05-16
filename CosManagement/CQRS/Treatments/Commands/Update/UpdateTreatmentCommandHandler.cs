using AutoMapper;
using CosManagement.CQRS.Common.Commands;
using CosManagement.Database;
using CosManagement.Dtos.Treatments;
using CosManagement.Entities;
using CosManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CosManagement.CQRS.Treatments.Commands.Update;

public class UpdateTreatmentCommandHandler : UpdateBaseHandler<UpdateTreatmentCommand, Treatment, UpdateTreatmentDto>
{
	private readonly ApplicationDbContext _context;

	public UpdateTreatmentCommandHandler(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService) : base(context, mapper, currentUserService)
	{
		_context = context;
	}

	public override IQueryable<Treatment> GetResources()
	{
		return _context.Treatments.Include(t => t.Category);
	}
}