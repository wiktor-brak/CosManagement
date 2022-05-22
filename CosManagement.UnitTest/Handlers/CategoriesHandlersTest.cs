using AutoMapper;
using CosManagement.Common.Models;
using CosManagement.CQRS.Categories.Commands.Create;
using CosManagement.CQRS.Categories.Commands.Delete;
using CosManagement.CQRS.Categories.Commands.Update;
using CosManagement.CQRS.Categories.Queries.GetCategory;
using CosManagement.CQRS.Categories.Queries.GetCategories;
using CosManagement.Database;
using CosManagement.Dtos.Categories;
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

public class CategoriesHandlersTest : IDisposable
{
	private readonly ApplicationDbContext _context;
	private readonly string _userId;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public CategoriesHandlersTest()
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
	}

	[Fact]
	public async Task GetHandler_WithCorrectData_ShouldReturnGetCategoryDto()
	{
		var category = new Category
		{
			Name = "Test Category",
			OwnerId = _userId
		};
		_context.Categories.Add(category);

		_context.SaveChanges();

		var handler = new GetCategoryQueryHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		var result = await handler.Handle(new GetCategoryQuery { Id = category.Id }, token);

		result.Should().BeOfType(typeof(GetCategoryDto));
	}

	[Fact]
	public async Task GetHandler_IncorrectId_ShouldThrowNotFoundException()
	{
		var handler = new GetCategoryQueryHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		Func<Task> act = async () => await handler.Handle(new GetCategoryQuery { Id = Guid.NewGuid() }, token);

		await act.Should().ThrowAsync<NotFoundException>();
	}

	[Fact]
	public async Task GetHandler_IncorrectOwnerId_ShouldThrowForbiddenAccessException()
	{
		var category = new Category
		{
			Name = "Test Name",
			OwnerId = Guid.NewGuid().ToString()
		};
		_context.Categories.Add(category);

		_context.SaveChanges();

		var handler = new GetCategoryQueryHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		Func<Task> act = async () => await handler.Handle(new GetCategoryQuery { Id = category.Id }, token);

		await act.Should().ThrowAsync<ForbiddenAccessException>();
	}

	[Fact]
	public async Task GetAllHandler_WithCorrectData_ShouldReturnPaginatedListOfGetCategoryDto()
	{
		var category = new Category
		{
			Name = "Test Name",
			OwnerId = _userId
		};
		_context.Categories.Add(category);

		_context.SaveChanges();

		var handler = new GetCategoriesWithPaginationQueryHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		var result = await handler.Handle(new GetCategoriesWithPaginationQuery(), token);

		result.Should().BeOfType(typeof(PaginatedList<GetCategoryDto>));
		result.Items.Find(g => g.Id == category.Id).Should().NotBeNull();
	}

	[Fact]
	public async Task GetAllHandler_IncorrectGetResource_ShouldThrowNotFoundException()
	{
		var token = new CancellationTokenSource().Token;
		var mockHandler = new Mock<GetCategoriesWithPaginationQueryHandler>(_context, _mapper, _currentUserService);

		mockHandler.Setup(x => x.GetResource()).Returns<Category>(null);

		Func<Task> act = async () => await mockHandler.Object.Handle(new GetCategoriesWithPaginationQuery(), token);

		await act.Should().ThrowAsync<NotFoundException>();
	}

	[Fact]
	public async Task GetAllHandler_IncorrectOwnerIdInResource_ShouldThrowForbiddenAccessException()
	{
		var category = new Category
		{
			Name = "Test Name",
			OwnerId = Guid.NewGuid().ToString()
		};
		_context.Categories.Add(category);

		_context.SaveChanges();

		var token = new CancellationTokenSource().Token;
		var mockHandler = new Mock<GetCategoriesWithPaginationQueryHandler>(_context, _mapper, _currentUserService);

		mockHandler.Setup(x => x.GetResource()).Returns(_context.Categories);

		Func<Task> act = async () => await mockHandler.Object.Handle(new GetCategoriesWithPaginationQuery(), token);

		await act.Should().ThrowAsync<ForbiddenAccessException>();
	}

	[Fact]
	public async Task CreateHandler_WithCorrectData_ShouldReturnCreateCategoryDto()
	{
		var handler = new CreateCategoryCommandHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		var name = "TestName";

		var result = await handler.Handle(new CreateCategoryCommand
		{
			Name = name
		}, token);

		result.Should().BeOfType(typeof(CreateCategoryDto));
		result.Name.Should().Be(name);
	}

	[Fact]
	public async Task CreateHandler_WithInorrectData_ShouldThrowValidationException()
	{
		var handler = new CreateCategoryCommandHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		Func<Task> act = async () => await handler.Handle(new CreateCategoryCommand(), token);

		await act.Should().ThrowAsync<DbUpdateException>();
	}

	[Fact]
	public async Task UpdateHandler_WithCorrectData_ShouldReturnUnit()
	{
		var category = new Category
		{
			Name = "TestName",
			OwnerId = _userId
		};
		_context.Categories.Add(category);

		_context.SaveChanges();

		var handler = new UpdateCategoryCommandHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		var result = await handler.Handle(new UpdateCategoryCommand
		{
			Dto = new UpdateCategoryDto
			{
				Name = "Updated Name"
			},
			Id = category.Id
		}, token);

		result.Should().BeOfType(typeof(Unit));
		category.Name.Should().Be("Updated Name");
	}

	[Fact]
	public async Task UpdateHandler_WithEmptyCategory_ShouldReturnNotFoundException()
	{
		var handler = new UpdateCategoryCommandHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		Func<Task> act = async () => await handler.Handle(new UpdateCategoryCommand
		{
			Dto = new UpdateCategoryDto
			{
				Name = "UpdatedName",
			},
			Id = Guid.NewGuid()
		}, token);

		await act.Should().ThrowAsync<NotFoundException>();
	}

	[Fact]
	public async Task UpdateHandler_WithIncorrectOwnerId_ShouldReturnForbiddenAccessException()
	{
		var category = new Category
		{
			Name = "Test Name"
		};
		_context.Categories.Add(category);

		_context.SaveChanges();

		var handler = new UpdateCategoryCommandHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		Func<Task> act = async () => await handler.Handle(new UpdateCategoryCommand
		{
			Dto = new UpdateCategoryDto
			{
				Name = "UpdatedName"
			},
			Id = category.Id
		}, token);

		await act.Should().ThrowAsync<ForbiddenAccessException>();
	}

	[Fact]
	public async Task DeleteHandler_WithCorrectData_ShouldReturnUnit()
	{
		var category = new Category
		{
			Name = "TestName",
			OwnerId = _userId
		};
		_context.Categories.Add(category);

		_context.SaveChanges();

		var handler = new DeleteCategoryCommandHandler(_context, _currentUserService);
		var token = new CancellationTokenSource().Token;

		var result = await handler.Handle(new DeleteCategoryCommand { Id = category.Id }, token);

		result.Should().BeOfType(typeof(Unit));
	}

	[Fact]
	public async Task DeleteHandler_WithEmptyCategory_ShouldReturnNotFound()
	{
		var handler = new DeleteCategoryCommandHandler(_context, _currentUserService);
		var token = new CancellationTokenSource().Token;

		Func<Task> act = async () => await handler.Handle(new DeleteCategoryCommand { Id = Guid.NewGuid() }, token);

		await act.Should().ThrowAsync<NotFoundException>();
	}

	[Fact]
	public async Task DeleteHandler_WithIncorrectOwnerId_ShouldReturnForbiddenAccessException()
	{
		var category = new Category
		{
			Name = "TestName"
		};
		_context.Categories.Add(category);

		_context.SaveChanges();

		var handler = new DeleteCategoryCommandHandler(_context, _currentUserService);
		var token = new CancellationTokenSource().Token;

		Func<Task> act = async () => await handler.Handle(new DeleteCategoryCommand { Id = category.Id }, token);

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