using AutoMapper;
using CosManagement.Common.Models;
using CosManagement.CQRS.Clients.Commands.Create;
using CosManagement.CQRS.Clients.Commands.Delete;
using CosManagement.CQRS.Clients.Commands.Update;
using CosManagement.CQRS.Clients.Queries.GetClient;
using CosManagement.CQRS.Clients.Queries.GetClientsWithPagination;
using CosManagement.Database;
using CosManagement.Dtos.Clients;
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

public class ClientHandlersTest : IDisposable
{
	private readonly ApplicationDbContext _context;
	private readonly string _userId;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public ClientHandlersTest()
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
	public async Task GetHandler_WithCorrectData_ShouldReturnGetClientDto()
	{
		var client = new Client
		{
			FirstName = "TestFirstName",
			LastName = "TestLastName",
			Email = "test@email.com",
			AdditionalInformations = "TestInfo",
			Phone = "111222333",
			OwnerId = _userId
		};
		_context.Clients.Add(client);

		_context.SaveChanges();

		var handler = new GetClientQueryHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		var result = await handler.Handle(new GetClientQuery { Id = client.Id }, token);

		result.Should().BeOfType(typeof(GetClientDto));
	}

	[Fact]
	public async Task GetHandler_IncorrectId_ShouldThrowNotFoundException()
	{
		var handler = new GetClientQueryHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		Func<Task> act = async () => await handler.Handle(new GetClientQuery { Id = Guid.NewGuid() }, token);

		await act.Should().ThrowAsync<NotFoundException>();
	}

	[Fact]
	public async Task GetHandler_IncorrectOwnerId_ShouldThrowForbiddenAccessException()
	{
		var client = new Client
		{
			FirstName = "TestFirstName",
			LastName = "TestLastName",
			Email = "test@email.com",
			AdditionalInformations = "TestInfo",
			Phone = "111222333",
			OwnerId = Guid.NewGuid().ToString()
		};
		_context.Clients.Add(client);

		_context.SaveChanges();

		var handler = new GetClientQueryHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		Func<Task> act = async () => await handler.Handle(new GetClientQuery { Id = client.Id }, token);

		await act.Should().ThrowAsync<ForbiddenAccessException>();
	}

	[Fact]
	public async Task GetAllHandler_WithCorrectData_ShouldReturnPaginatedListOfGetClientDto()
	{
		var client = new Client
		{
			FirstName = "TestFirstName",
			LastName = "TestLastName",
			Email = "test@email.com",
			AdditionalInformations = "TestInfo",
			Phone = "111222333",
			OwnerId = _userId
		};
		_context.Clients.Add(client);

		_context.SaveChanges();

		var handler = new GetClientsWithPaginationQueryHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		var result = await handler.Handle(new GetClientsWithPaginationQuery(), token);

		result.Should().BeOfType(typeof(PaginatedList<GetClientDto>));
		result.Items.Find(g => g.Id == client.Id).Should().NotBeNull();
	}

	[Fact]
	public async Task GetAllHandler_IncorrectGetResource_ShouldThrowNotFoundException()
	{
		var token = new CancellationTokenSource().Token;
		var mockHandler = new Mock<GetClientsWithPaginationQueryHandler>(_context, _mapper, _currentUserService);

		mockHandler.Setup(x => x.GetResource()).Returns<Client>(null);

		Func<Task> act = async () => await mockHandler.Object.Handle(new GetClientsWithPaginationQuery(), token);

		await act.Should().ThrowAsync<NotFoundException>();
	}

	[Fact]
	public async Task GetAllHandler_IncorrectOwnerIdInResource_ShouldThrowForbiddenAccessException()
	{
		var client = new Client
		{
			FirstName = "TestFirstName",
			LastName = "TestLastName",
			Email = "test@email.com",
			AdditionalInformations = "TestInfo",
			Phone = "111222333",
			OwnerId = Guid.NewGuid().ToString()
		};
		_context.Clients.Add(client);

		_context.SaveChanges();

		var token = new CancellationTokenSource().Token;
		var mockHandler = new Mock<GetClientsWithPaginationQueryHandler>(_context, _mapper, _currentUserService);

		mockHandler.Setup(x => x.GetResource()).Returns(_context.Clients);

		Func<Task> act = async () => await mockHandler.Object.Handle(new GetClientsWithPaginationQuery(), token);

		await act.Should().ThrowAsync<ForbiddenAccessException>();
	}

	[Fact]
	public async Task CreateHandler_WithCorrectData_ShouldReturnCreateClientDto()
	{
		var handler = new CreateClientCommandHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		var firstName = "TestFirstName";
		var lastName = "TestLastName";
		var email = "test@email.com";
		var additionalInformations = "TestInfo";
		var phone = "111222333";

		var result = await handler.Handle(new CreateClientCommand
		{
			FirstName = firstName,
			LastName = lastName,
			Email = email,
			AdditionalInformations = additionalInformations,
			Phone = phone
		}, token);

		result.Should().BeOfType(typeof(CreateClientDto));
		result.FirstName.Should().Be(firstName);
		result.LastName.Should().Be(lastName);
		result.Email.Should().Be(email);
		result.Phone.Should().Be(phone);
		result.AdditionalInformations.Should().Be(additionalInformations);
	}

	[Fact]
	public async Task CreateHandler_WithInorrectData_ShouldThrowValidationException()
	{
		var handler = new CreateClientCommandHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		Func<Task> act = async () => await handler.Handle(new CreateClientCommand(), token);

		await act.Should().ThrowAsync<DbUpdateException>();
	}

	[Fact]
	public async Task UpdateHandler_WithCorrectData_ShouldReturnUnit()
	{
		var client = new Client
		{
			FirstName = "TestFirstName",
			LastName = "TestLastName",
			Email = "test@email.com",
			AdditionalInformations = "TestInfo",
			Phone = "111222333",
			OwnerId = _userId
		};
		_context.Clients.Add(client);

		_context.SaveChanges();

		var handler = new UpdateClientCommandHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		var result = await handler.Handle(new UpdateClientCommand
		{
			Dto = new UpdateClientDto
			{
				FirstName = "UpdatedTestFirstName",
				LastName = "UpdatedTestLastName",
				Email = "Updatedtest@email.com",
				AdditionalInformations = "UpdatedTestInfo",
				Phone = "444555666"
			},
			Id = client.Id
		}, token);

		result.Should().BeOfType(typeof(Unit));
	}

	[Fact]
	public async Task UpdateHandler_WithEmptyClient_ShouldReturnNotFoundException()
	{
		var handler = new UpdateClientCommandHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		Func<Task> act = async () => await handler.Handle(new UpdateClientCommand
		{
			Dto = new UpdateClientDto
			{
				FirstName = "UpdatedTestFirstName",
				LastName = "UpdatedTestLastName",
				Email = "Updatedtest@email.com",
				AdditionalInformations = "UpdatedTestInfo",
				Phone = "444555666"
			},
			Id = Guid.NewGuid()
		}, token);

		await act.Should().ThrowAsync<NotFoundException>();
	}

	[Fact]
	public async Task UpdateHandler_WithIncorrectOwnerId_ShouldReturnForbiddenAccessException()
	{
		var client = new Client
		{
			FirstName = "TestFirstName",
			LastName = "TestLastName",
			Email = "test@email.com",
			AdditionalInformations = "TestInfo",
			Phone = "111222333",
		};
		_context.Clients.Add(client);

		_context.SaveChanges();

		var handler = new UpdateClientCommandHandler(_context, _mapper, _currentUserService);
		var token = new CancellationTokenSource().Token;

		Func<Task> act = async () => await handler.Handle(new UpdateClientCommand
		{
			Dto = new UpdateClientDto
			{
				FirstName = "UpdatedTestFirstName",
				LastName = "UpdatedTestLastName",
				Email = "Updatedtest@email.com",
				AdditionalInformations = "UpdatedTestInfo",
				Phone = "444555666"
			},
			Id = client.Id
		}, token);

		await act.Should().ThrowAsync<ForbiddenAccessException>();
	}

	[Fact]
	public async Task DeleteHandler_WithCorrectData_ShouldReturnUnit()
	{
		var client = new Client
		{
			FirstName = "TestFirstName",
			LastName = "TestLastName",
			Email = "test@email.com",
			AdditionalInformations = "TestInfo",
			Phone = "111222333",
			OwnerId = _userId
		};
		_context.Clients.Add(client);

		_context.SaveChanges();

		var handler = new DeleteClientCommandHandler(_context, _currentUserService);
		var token = new CancellationTokenSource().Token;

		var result = await handler.Handle(new DeleteClientCommand { Id = client.Id }, token);

		result.Should().BeOfType(typeof(Unit));
	}

	[Fact]
	public async Task DeleteHandler_WithEmptyClient_ShouldReturnNotFound()
	{
		var handler = new DeleteClientCommandHandler(_context, _currentUserService);
		var token = new CancellationTokenSource().Token;

		Func<Task> act = async () => await handler.Handle(new DeleteClientCommand { Id = Guid.NewGuid() }, token);

		await act.Should().ThrowAsync<NotFoundException>();
	}

	[Fact]
	public async Task DeleteHandler_WithIncorrectOwnerId_ShouldReturnForbiddenAccessException()
	{
		var client = new Client
		{
			FirstName = "TestFirstName",
			LastName = "TestLastName",
			Email = "test@email.com",
			AdditionalInformations = "TestInfo",
			Phone = "111222333",
		};
		_context.Clients.Add(client);

		_context.SaveChanges();

		var handler = new DeleteClientCommandHandler(_context, _currentUserService);
		var token = new CancellationTokenSource().Token;

		Func<Task> act = async () => await handler.Handle(new DeleteClientCommand { Id = client.Id }, token);

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