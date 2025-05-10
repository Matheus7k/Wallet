using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Wallet.Application.Commands.v1.Users.AddUser;
using Wallet.CrossCutting.Exception;
using Wallet.Domain.Entities.v1;
using Wallet.Domain.Interfaces.v1.Repositories;

namespace Wallet.Tests.Commands.v1.Users.AddUser;

[TestClass]
public class AddUserCommandHandlerTest
{
    private Mock<IUserCommandRepository> _userCommandRepositoryMock;
    private Mock<IMapper> _mapperMock;
    private Mock<ILogger<AddUserCommandHandler>> _loggerMock;
    private AddUserCommandHandler _handler;

    [TestInitialize]
    public void Setup()
    {
        _userCommandRepositoryMock = new Mock<IUserCommandRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<AddUserCommandHandler>>();
        _handler = new AddUserCommandHandler(_userCommandRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [TestMethod]
    public async Task ShouldAddUserWhenEmailDoesNotExist()
    {
        var command = new AddUserCommand
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "Pass@123",
            BirthDate = new DateTime(1990, 1, 1),
            Address = new Address { Street = "123 Street", Number = 1, City = "Test City", Country = "Test Country" }
        };

        _userCommandRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(command.Email))
            .ReturnsAsync((User)null);

        _mapperMock.Setup(map => map.Map<User>(command)).Returns(new User { Id = Guid.NewGuid(), Name = command.Name, Email = command.Email });
        
        await _handler.Handle(command, CancellationToken.None);
        
        _userCommandRepositoryMock.Verify(repo => repo.GetUserByEmailAsync(command.Email), Times.Once);
        _userCommandRepositoryMock.Verify(repo => repo.AddUserAsync(It.IsAny<User>(), It.IsAny<UserWallet>()), Times.Once);
    }

    [TestMethod]
    public async Task ShouldThrowConflictExceptionWhenEmailAlreadyExists()
    {
        var command = new AddUserCommand
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "Pass@123",
            BirthDate = new DateTime(1990, 1, 1),
            Address = new Address { Street = "123 Street", Number = 1, City = "Test City", Country = "Test Country" }
        };

        var existingUser = new User { Email = command.Email };

        _userCommandRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(command.Email))
            .ReturnsAsync(existingUser);
        
        await Assert.ThrowsExactlyAsync<ConflictException>(async () => await _handler.Handle(command, CancellationToken.None));
    }

    [TestMethod]
    public async Task ShouldHashPasswordCorrectly()
    {
        var command = new AddUserCommand
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "Pass@123",
            BirthDate = new DateTime(1990, 1, 1),
            Address = new Address { Street = "123 Street", Number = 1, City = "Test City", Country = "Test Country" }
        };

        _userCommandRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User)null);

        var mappedUser = new User { Id = Guid.NewGuid(), Name = command.Name, Email = command.Email };
        _mapperMock.Setup(map => map.Map<User>(command)).Returns(mappedUser);
        
        await _handler.Handle(command, CancellationToken.None);
        
        Assert.IsFalse(string.IsNullOrEmpty(mappedUser.Password));
        Assert.IsTrue(BCrypt.Net.BCrypt.Verify(command.Password, mappedUser.Password));
    }

    [TestMethod]
    public async Task ShouldAddUserWithZeroBalanceWalletWhenProcessingSuccessfully()
    {
        var command = new AddUserCommand
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "Pass@123",
            BirthDate = new DateTime(1990, 1, 1),
            Address = new Address { Street = "123 Street", Number = 1, City = "Test City", Country = "Test Country" }
        };

        var mappedUser = new User { Id = Guid.NewGuid(), Name = command.Name, Email = command.Email };
        _mapperMock.Setup(map => map.Map<User>(command)).Returns(mappedUser);

        _userCommandRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User)null);
        
        await _handler.Handle(command, CancellationToken.None);
        
        _userCommandRepositoryMock.Verify(repo => repo.AddUserAsync(mappedUser, It.Is<UserWallet>(wallet => wallet.Balance == 0.00m && wallet.UserId == mappedUser.Id)), Times.Once);
    }
}