using AutoMapper;
using CosManagement.CQRS.Common;
using CosManagement.Database;
using CosManagement.Dtos.Treatments;
using CosManagement.Entities;
using CosManagement.Exceptions;
using CosManagement.Interfaces;

namespace CosManagement.CQRS.Treatments.Commands.Create;

public class CreateTreatmentCommandHandler : CreateBaseHandler<CreateTreatmentCommand, CreateTreatmentDto, Treatment>
{
	private readonly ApplicationDbContext _context;

	public CreateTreatmentCommandHandler(ApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
		: base(context, mapper, currentUserService)
	{
		_context = context;
	}

	public override void AppendAdditionalProperty(Treatment resource, CreateTreatmentCommand request)
	{
		Category? category;

		if (resource.CategoryId == Guid.Empty)
		{
			category = _context.Categories.FirstOrDefault(c => c.Name!.Equals("Common"));

			if (category != null)
			{
				resource.CategoryId = category!.Id;
				resource.Category = category;
			}

			return;
		}

		category = _context.Categories.FirstOrDefault(c => c.Id == resource.CategoryId);
		resource.Category = category;
	}

	public override void AppendAdditionalValidation(Treatment resource)
	{
		var category = _context.Categories.FirstOrDefault(c => c.Id == resource.CategoryId);

		if (category is null)
		{
			throw new NotFoundException($"{NotFoundException.MessagePrefix} category");
		}
	}
}