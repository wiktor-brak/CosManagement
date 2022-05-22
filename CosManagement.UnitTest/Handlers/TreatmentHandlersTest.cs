using AutoMapper;
using CosManagement.Common.Models;
using CosManagement.CQRS.Treatments.Commands.Create;
using CosManagement.CQRS.Treatments.Commands.Delete;
using CosManagement.CQRS.Treatments.Commands.Update;
using CosManagement.CQRS.Treatments.Queries;
using CosManagement.CQRS.Treatments.Queries.GetTreatmentWithPagination;
using CosManagement.Database;
using CosManagement.Dtos.Treatments;
using CosManagement.Entities;
using CosManagement.Exceptions;
using CosManagement.Interfaces;
using CosManagement.Mappings;
using Duende.IdentityServer.EntityFramework.Options;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CosManagement.UnitTest.Handlers;

public class TreatmentHandlersTest : IDisposable
{
	private readonly ApplicationDbContext _context;
	private readonly string _userId;
	private readonly Category? _category;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public TreatmentHandlersTest()
	{
		_userId = Guid.NewGuid().ToString();

		var currentUserServiceMock = new Mock<ICurrentUserService>();
		currentUserServiceMock.Setup(c => c.UserId).Returns(_userId);
		_currentUserService = currentUserServiceMock.Object;

		var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("testDb").Options;
		var operationalStoreOptions = Options.Create(new OperationalStoreOptions());
		_context = new ApplicationDbContext(options, operationalStoreOptions, _currentUserService);

		var configuration = new MapperConfiguration(config => config.AddProfile<MappingProfile>());
		_mapper = new Mapper(configuration);

		var category = new Category
		{
			Name = "TestCategory"
		};

		_context.Categories.Add(category);

		_context.SaveChanges();

		_category = category;
	}

	[Fact]
	public async Task GetHandler_WithCorrectData_ShouldReturnGetClientDto()
	{
		var treatment = new Treatment
		{
			Name = "TestName",
			BasePrice = 10m,
			OwnerId = _userId,
			CategoryId = _category!.Id,
			Category = _category
		};
		_context.Treatments.Add(treatment);

		_context.SaveChanges();

		var handler = new GetTreatmentQueryHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		var result = await handler.Handle(new GetTreatmentQuery { Id = treatment.Id }, token);

		result.Should().BeOfType(typeof(GetTreatmentDto));
	}

	[Fact]
	public async Task GetHandler_IncorrectId_ShouldThrowNotFoundException()
	{
		var handler = new GetTreatmentQueryHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		Func<Task> act = async () => await handler.Handle(new GetTreatmentQuery { Id = Guid.NewGuid() }, token);

		await act.Should().ThrowAsync<NotFoundException>();
	}

	[Fact]
	public async Task GetHandler_IncorrectOwnerId_ShouldThrowForbiddenAccessException()
	{
		var treatment = new Treatment
		{
			Name = "TestName",
			BasePrice = 10m,
			OwnerId = Guid.NewGuid().ToString(),
			CategoryId = _category!.Id,
			Category = _category
		};

		_context.Treatments.Add(treatment);

		_context.SaveChanges();

		var handler = new GetTreatmentQueryHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		Func<Task> act = async () => await handler.Handle(new GetTreatmentQuery { Id = treatment.Id }, token);

		await act.Should().ThrowAsync<ForbiddenAccessException>();
	}

	[Fact]
	public async Task GetAllHandler_WithCorrectData_ShouldReturnPaginatedListOfGetClientDto()
	{
		var treatment = new Treatment
		{
			Name = "TestName",
			BasePrice = 10m,
			OwnerId = _userId,
			CategoryId = _category!.Id,
			Category = _category
		};
		_context.Treatments.Add(treatment);

		_context.SaveChanges();

		var handler = new GetTreatmentsWithPaginationQueryHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		var result = await handler.Handle(new GetTreatmentsWithPaginationQuery(), token);

		result.Should().BeOfType(typeof(PaginatedList<GetTreatmentDto>));
		result.Items.Find(g => g.Id == treatment.Id).Should().NotBeNull();
	}

	[Fact]
	public async Task GetAllHandler_IncorrectGetResource_ShouldThrowNotFoundException()
	{
		var token = new CancellationTokenSource().Token;
		var mockHandler = new Mock<GetTreatmentsWithPaginationQueryHandler>(_context, _mapper, _currentUserService);

		mockHandler.Setup(x => x.GetResource()).Returns<Client>(null);

		Func<Task> act = async () => await mockHandler.Object.Handle(new GetTreatmentsWithPaginationQuery(), token);

		await act.Should().ThrowAsync<NotFoundException>();
	}

	[Fact]
	public async Task GetAllHandler_IncorrectOwnerIdInResource_ShouldThrowForbiddenAccessException()
	{
		var treatment = new Treatment
		{
			Name = "TestName",
			BasePrice = 10m,
			OwnerId = Guid.NewGuid().ToString(),
			CategoryId = _category!.Id,
			Category = _category
		};

		_context.Treatments.Add(treatment);

		_context.SaveChanges();

		var token = new CancellationTokenSource().Token;
		var mockHandler = new Mock<GetTreatmentsWithPaginationQueryHandler>(_context, _mapper, _currentUserService);

		mockHandler.Setup(x => x.GetResource()).Returns(_context.Treatments);

		Func<Task> act = async () => await mockHandler.Object.Handle(new GetTreatmentsWithPaginationQuery(), token);

		await act.Should().ThrowAsync<ForbiddenAccessException>();
	}

	[Fact]
	public async Task CreateHandler_WithCorrectData_ShouldReturnCreateClientDto()
	{
		var handler = new CreateTreatmentCommandHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		var name = "TestFirstName";
		var basePrice = 10m;

		var result = await handler.Handle(new CreateTreatmentCommand
		{
			Name = name,
			BasePrice = basePrice,
			CategoryId = _category!.Id,
		}, token);

		result.Should().BeOfType(typeof(CreateTreatmentDto));
		result.Name.Should().Be(name);
		result.BasePrice.Should().Be(basePrice);
	}

	[Fact]
	public async Task CreateHandler_WithInorrectData_ShouldThrowValidationException()
	{
		var handler = new CreateTreatmentCommandHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		Func<Task> act = async () => await handler.Handle(new CreateTreatmentCommand(), token);

		await act.Should().ThrowAsync<NotFoundException>();
	}

	[Fact]
	public async Task UpdateHandler_WithCorrectData_ShouldReturnUnit()
	{
		var treatment = new Treatment
		{
			Name = "TestName",
			BasePrice = 10m,
			OwnerId = _userId,
			CategoryId = _category!.Id,
			Category = _category
		};

		_context.Treatments.Add(treatment);

		_context.SaveChanges();

		var handler = new UpdateTreatmentCommandHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		var result = await handler.Handle(new UpdateTreatmentCommand
		{
			Dto = new UpdateTreatmentDto
			{
				Name = "UpdatedTestName",
				BasePrice = 20m,
				CategoryId = _category!.Id,
			},
			Id = treatment.Id
		}, token);

		result.Should().BeOfType(typeof(Unit));
	}

	[Fact]
	public async Task UpdateHandler_WithWrongTreatmentId_ShouldReturnNotFoundException()
	{
		var handler = new UpdateTreatmentCommandHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		Func<Task> act = async () => await handler.Handle(new UpdateTreatmentCommand
		{
			Dto = new UpdateTreatmentDto
			{
				Name = "UpdatedTestName",
				BasePrice = 20m,
				CategoryId = _category!.Id,
			},
			Id = Guid.NewGuid()
		}, token);

		await act.Should().ThrowAsync<NotFoundException>();
	}

	[Fact]
	public async Task UpdateHandler_WithIncorrectOwnerId_ShouldReturnForbiddenAccessException()
	{
		var treatment = new Treatment
		{
			Name = "TestName",
			BasePrice = 10m,
			CategoryId = _category!.Id,
			Category = _category
		};

		_context.Treatments.Add(treatment);

		_context.SaveChanges();

		var handler = new UpdateTreatmentCommandHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		Func<Task> act = async () => await handler.Handle(new UpdateTreatmentCommand
		{
			Dto = new UpdateTreatmentDto
			{
				Name = "UpdatedTestName",
				BasePrice = 30m,
				CategoryId = _category!.Id,
			},
			Id = treatment.Id
		}, token);

		await act.Should().ThrowAsync<ForbiddenAccessException>();
	}

	[Fact]
	public async Task DeleteHandler_WithCorrectData_ShouldReturnUnit()
	{
		var treatment = new Treatment
		{
			Name = "TestName",
			BasePrice = 10m,
			OwnerId = _userId,
			CategoryId = _category!.Id,
			Category = _category
		};

		_context.Treatments.Add(treatment);

		_context.SaveChanges();

		var handler = new DeleteTreatmentCommandHandler(_context, _currentUserService);
		var token = new CancellationTokenSource().Token;

		var result = await handler.Handle(new DeleteTreatmentCommand { Id = treatment.Id }, token);

		result.Should().BeOfType(typeof(Unit));
	}

	[Fact]
	public async Task DeleteHandler_WithEmptyTreatment_ShouldReturnNotFound()
	{
		var handler = new DeleteTreatmentCommandHandler(_context, _currentUserService);
		var token = new CancellationTokenSource().Token;

		Func<Task> act = async () => await handler.Handle(new DeleteTreatmentCommand { Id = Guid.NewGuid() }, token);

		await act.Should().ThrowAsync<NotFoundException>();
	}

	[Fact]
	public async Task DeleteHandler_WithIncorrectOwnerId_ShouldReturnForbiddenAccessException()
	{
		var treatment = new Treatment
		{
			Name = "TestName",
			BasePrice = 10m,
			CategoryId = _category!.Id,
			Category = _category
		};

		_context.Treatments.Add(treatment);

		_context.SaveChanges();

		var handler = new DeleteTreatmentCommandHandler(_context, _currentUserService);
		var token = new CancellationTokenSource().Token;

		Func<Task> act = async () => await handler.Handle(new DeleteTreatmentCommand { Id = treatment.Id }, token);

		await act.Should().ThrowAsync<ForbiddenAccessException>();
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			_context.Dispose();
		}
	}
}