using CosManagement.CQRS.Common.Commands;
using CosManagement.Database;
using CosManagement.Entities;
using CosManagement.Interfaces;

namespace CosManagement.CQRS.Treatments.Commands.Delete;

public class DeleteTreatmentCommandHandler : DeleteBaseHandler<DeleteTreatmentCommand, Treatment>
{
	public DeleteTreatmentCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService) : base(context, currentUserService)
	{ }
}