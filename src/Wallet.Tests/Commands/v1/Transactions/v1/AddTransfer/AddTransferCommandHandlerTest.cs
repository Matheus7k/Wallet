using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Wallet.Application.Commands.v1.Transactions.AddTransfer;
using Wallet.CrossCutting.Exception;
using Wallet.Domain.Entities.v1;
using Wallet.Domain.Enums.v1;
using Wallet.Domain.Interfaces.v1.Factories;
using Wallet.Domain.Interfaces.v1.Repositories;
using Wallet.Domain.ValueObjects.v1;

namespace Wallet.Tests.Commands.v1.Transactions.v1.AddTransfer;

[TestClass]
public class AddTransferCommandHandlerTest
{
    private Mock<IUserCommandRepository> _userCommandRepositoryMock = null!;
    private Mock<IWalletTransactionFactory> _walletTransactionFactoryMock = null!;
    private Mock<ILogger<AddTransferCommandHandler>> _loggerMock = null!;
    private AddTransferCommandHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _userCommandRepositoryMock = new Mock<IUserCommandRepository>();
        _walletTransactionFactoryMock = new Mock<IWalletTransactionFactory>();
        _loggerMock = new Mock<ILogger<AddTransferCommandHandler>>();

        _handler = new AddTransferCommandHandler(
            _userCommandRepositoryMock.Object,
            _walletTransactionFactoryMock.Object,
            _loggerMock.Object
        );
    }

    [TestMethod]
    public async Task ShouldExecuteTransferSuccessfullyWhenValidRequest()
    {
        var command = new AddTransferCommand
        {
            ToEmail = "receiver@example.com",
            Amount = 100,
        }.SetFromEmail("sender@example.com");

        var senderWallet = new UserWallet
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Balance = 200,
            IsActive = true,
        };

        var receiverWallet = new UserWallet
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Balance = 50,
            IsActive = true,
        };

        SetupUserRepositoryMock(command, senderWallet, receiverWallet);

        _walletTransactionFactoryMock.Setup(f =>
            f.CreateWalletTransaction(TransactionType.Transfer, It.IsAny<WalletTransactionValueObject>()));

        await _handler.Handle(command, CancellationToken.None);

        Assert.AreEqual(100, senderWallet.Balance);
        Assert.AreEqual(150, receiverWallet.Balance);
        _userCommandRepositoryMock.Verify(r => r.UpdateTransferWalletsAsync(It.IsAny<WalletTransferValueObject>()), Times.Once);
    }

    [TestMethod]
    public async Task ShouldThrowNotFoundExceptionWhenDestinationUserDoesNotExist()
    {
        var command = new AddTransferCommand
        {
            ToEmail = "nonexistent@example.com",
            Amount = 100,
        }.SetFromEmail("sender@example.com");

        _userCommandRepositoryMock.Setup(r => r.GetUserByEmailAsync(command.ToEmail)).ReturnsAsync((User?)null);

        await Assert.ThrowsExactlyAsync<NotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));
    }

    [TestMethod]
    public async Task ShouldThrowBadRequestExceptionWhenWalletIsInactive()
    {
        var command = new AddTransferCommand
        {
            ToEmail = "receiver@example.com",
            Amount = 100,
        }.SetFromEmail("sender@example.com");

        var senderWallet = new UserWallet
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Balance = 200,
            IsActive = false,
        };

        var receiverWallet = new UserWallet
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Balance = 50,
            IsActive = true,
        };

        SetupUserRepositoryMock(command, senderWallet, receiverWallet);

        await Assert.ThrowsExactlyAsync<BadRequestException>(async () => await _handler.Handle(command, CancellationToken.None));
    }

    [TestMethod]
    public async Task ShouldThrowConflictExceptionWhenInsufficientBalance()
    {
        var command = new AddTransferCommand
        {
            ToEmail = "receiver@example.com",
            Amount = 300,
        }.SetFromEmail("sender@example.com");

        var senderWallet = new UserWallet
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Balance = 200,
            IsActive = true,
        };

        var receiverWallet = new UserWallet
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Balance = 50,
            IsActive = true,
        };

        SetupUserRepositoryMock(command, senderWallet, receiverWallet);

        await Assert.ThrowsExactlyAsync<ConflictException>(async () => await _handler.Handle(command, CancellationToken.None));
    }

    private void SetupUserRepositoryMock(AddTransferCommand command, UserWallet senderWallet, UserWallet receiverWallet)
    {
        _userCommandRepositoryMock.Setup(r => r.GetUserByEmailAsync(command.ToEmail))
            .ReturnsAsync(new User { Email = command.ToEmail });

        _userCommandRepositoryMock.Setup(r => r.GetUserIdByEmailAsync(command.FromEmail))
            .ReturnsAsync(senderWallet.UserId);

        _userCommandRepositoryMock.Setup(r => r.GetUserIdByEmailAsync(command.ToEmail))
            .ReturnsAsync(receiverWallet.UserId);

        _userCommandRepositoryMock.Setup(r => r.GetWalletByIdAsync(senderWallet.UserId))
            .ReturnsAsync(senderWallet);

        _userCommandRepositoryMock.Setup(r => r.GetWalletByIdAsync(receiverWallet.UserId))
            .ReturnsAsync(receiverWallet);
    }
}