using CosManagement.CQRS.Clients.Commands.Create;
using CosManagement.CQRS.Clients.Commands.Update;
using CosManagement.CQRS.Clients.Queries.GetClientsWithPagination;
using CosManagement.Dtos.Clients;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CosManagement.UnitTest.Validators;

public class ClientValidatiorsTest
{
	[Fact]
	public async Task CreateValidator_WithCorrectData_ShouldReturnIsValidTrue()
	{
		var validator = new CreateClientCommandValidator();
		var command = new CreateClientCommand
		{
			FirstName = "TestFirstName",
			LastName = "TestLastName",
			Email = "test@email.com",
			AdditionalInformations = "TestInfo",
			Phone = "111222333",
		};

		var validationResult = await validator.ValidateAsync(command);

		validationResult.IsValid.Should().BeTrue();
	}

	[Fact]
	public async Task CreateValidator_WithInorrectData_ShouldReturnIsValidFalseAndErrors()
	{
		var validator = new CreateClientCommandValidator();
		var command = new CreateClientCommand
		{
			FirstName = "",
			Email = "test",
			Phone = "111222333444555666",
		};

		var validationResult = await validator.ValidateAsync(command);

		var errorList = validationResult.Errors.AsEnumerable().Select(e => e.ErrorMessage);

		validationResult.IsValid.Should().BeFalse();
		errorList.Should().Contain("First name cannot be empty");
		errorList.Should().Contain("Last name cannot be empty");
		errorList.Should().Contain("Provide correct email address");
		errorList.Should().Contain("Phone number length must not exceed 12.");
	}

	[Fact]
	public async Task UpdateValidator_WithCorrectData_ShouldReturnIsValidTrue()
	{
		var validator = new UpdateClientCommandValidator();
		var command = new UpdateClientCommand
		{
			Dto = new UpdateClientDto
			{
				FirstName = "TestFirstName",
				LastName = "TestLastName",
				Email = "test@email.com",
				AdditionalInformations = "TestInfo",
				Phone = "111222333",
			}
		};

		var validationResult = await validator.ValidateAsync(command);

		validationResult.IsValid.Should().BeTrue();
	}

	[Fact]
	public async Task UpdateValidator_WithInorrectData_ShouldReturnIsValidFalseAndErrors()
	{
		var validator = new UpdateClientCommandValidator();
		var command = new UpdateClientCommand
		{
			Dto = new UpdateClientDto
			{
				FirstName = "",
				Email = "test",
				Phone = "111222333444555666",
			}
		};

		var validationResult = await validator.ValidateAsync(command);

		var errorList = validationResult.Errors.AsEnumerable().Select(e => e.ErrorMessage);

		validationResult.IsValid.Should().BeFalse();
		errorList.Should().Contain("First name cannot be empty");
		errorList.Should().Contain("Last name cannot be empty");
		errorList.Should().Contain("Provide correct email address");
		errorList.Should().Contain("Phone number length must not exceed 12.");
	}

	[Fact]
	public async Task GetAllValidator_WithCorrectData_ShouldReturnIsValidTrue()
	{
		var validator = new GetClientsWithPaginationQueryValidator();
		var command = new GetClientsWithPaginationQuery { PageNumber = 1, PageSize = 10 };
		var validationResult = await validator.ValidateAsync(command);

		validationResult.IsValid.Should().BeTrue();
	}

	[Fact]
	public async Task GetAllValidator_WithInorrectData_ShouldReturnIsValidFalseAndErrors()
	{
		var validator = new GetClientsWithPaginationQueryValidator();
		var command = new GetClientsWithPaginationQuery { PageNumber = -1, PageSize = 200 };

		var validationResult = await validator.ValidateAsync(command);

		var errorList = validationResult.Errors.AsEnumerable().Select(e => e.ErrorMessage);

		validationResult.IsValid.Should().BeFalse();
		errorList.Should().Contain("PageSize max value is 50");
		errorList.Should().Contain("PageNumber at least greater than or equal to 1.");
	}
}