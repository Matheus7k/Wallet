using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Wallet.Application.Commands.v1.Transactions.AddDeposit;
using Wallet.CrossCutting.Exception;
using Wallet.Domain.Entities.v1;
using Wallet.Domain.Enums.v1;
using Wallet.Domain.Interfaces.v1.Factories;
using Wallet.Domain.Interfaces.v1.Repositories;
using Wallet.Domain.ValueObjects.v1;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Wallet.Tests.Commands.v1.Transactions.v1.AddDeposit;

[TestClass]
public class AddDepositCommandHandlerTest
{
    private Mock<IUserCommandRepository> _userCommandRepositoryMock;
    private Mock<IWalletTransactionFactory> _walletTransactionFactoryMock;
    private Mock<ILogger<AddDepositCommandHandler>> _loggerMock;
    private AddDepositCommandHandler _handler;

    [TestInitialize]
    public void SetUp()
    {
        _userCommandRepositoryMock = new Mock<IUserCommandRepository>();
        _walletTransactionFactoryMock = new Mock<IWalletTransactionFactory>();
        _loggerMock = new Mock<ILogger<AddDepositCommandHandler>>();

        _handler = new AddDepositCommandHandler(
            _userCommandRepositoryMock.Object,
            _walletTransactionFactoryMock.Object,
            _loggerMock.Object
        );
    }

    [TestMethod]
    public async Task ShouldProcessDepositSuccessfullyWhenValidRequest()
    {
        var command = new AddDepositCommand { Email = "test@example.com", Amount = 100 };

        var userWallet = new UserWallet
        {
            Id = Guid.NewGuid(),
            IsActive = true,
            Balance = 50,
            UserId = Guid.NewGuid(),
        };

        var walletTransaction = new WalletTransaction
        {
            Id = Guid.NewGuid(),
            Amount = 100,
            Transaction = nameof(TransactionType.Deposit)
        };

        SetupUserRepositoryMock(command, userWallet);
        
        _walletTransactionFactoryMock.Setup(x => x.CreateWalletTransaction(
            TransactionType.Deposit,
            It.IsAny<WalletTransactionValueObject>()
        )).Returns(walletTransaction);

        await _handler.Handle(command, CancellationToken.None);

        _userCommandRepositoryMock.Verify(x => x.UpdateUserWalletAsync(It.Is<UserWallet>(w => w.Balance == 150), walletTransaction), Times.Once);
    }

    [TestMethod]
    public async Task ShouldThrowBadRequestExceptionWhenWalletIsInactive()
    {
        var command = new AddDepositCommand { Email = "test@example.com", Amount = 100 };

        var userWallet = new UserWallet
        {
            Id = Guid.NewGuid(),
            IsActive = false,
            UserId = Guid.NewGuid(),
        };

        SetupUserRepositoryMock(command, userWallet);

        await ThrowsExactlyAsync<BadRequestException>(async () => { await _handler.Handle(command, CancellationToken.None); });
    }

    [TestMethod]
    public async Task ShouldValidateWalletTransactionCreationWhenValidRequest()
    {
        var command = new AddDepositCommand { Email = "test@example.com", Amount = 200 };

        var userWallet = new UserWallet
        {
            Id = Guid.NewGuid(),
            IsActive = true,
            Balance = 50,
            UserId = Guid.NewGuid(),
        };

        SetupUserRepositoryMock(command, userWallet);

        await _handler.Handle(command, CancellationToken.None);

        _walletTransactionFactoryMock.Verify(x => x.CreateWalletTransaction(
            TransactionType.Deposit,
            It.Is<WalletTransactionValueObject>(v => v.Amount == 200 && v.FromWalletId == userWallet.Id)
        ), Times.Once);
    }

    private void SetupUserRepositoryMock(AddDepositCommand command, UserWallet userWallet)
    {
        _userCommandRepositoryMock.Setup(x => x.GetUserIdByEmailAsync(command.Email))
            .ReturnsAsync(userWallet.UserId);
        
        _userCommandRepositoryMock.Setup(x => x.GetWalletByIdAsync(userWallet.UserId))
            .ReturnsAsync(userWallet);
    }
}