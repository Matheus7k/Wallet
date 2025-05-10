using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Wallet.Application.Commands.v1.Authenticate.PostAuthenticate;
using Wallet.CrossCutting.Exception;
using Wallet.Domain.Entities.v1;
using Wallet.Domain.Interfaces.v1.Repositories;
using Wallet.Domain.Interfaces.v1.Services;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Wallet.Tests.Commands.v1.Authenticate.PostAuthenticate;

[TestClass]
public class PostAuthenticateCommandHandlerTest
{
    private Mock<IUserCommandRepository> _userCommandRepositoryMock;
    private Mock<ITokenService> _tokenServiceMock;
    private Mock<ILogger<PostAuthenticateCommandHandler>> _loggerMock;
    private PostAuthenticateCommandHandler _handler;

    [TestInitialize]
    public void Initialize()
    {
        _userCommandRepositoryMock = new Mock<IUserCommandRepository>();
        _tokenServiceMock = new Mock<ITokenService>();
        _loggerMock = new Mock<ILogger<PostAuthenticateCommandHandler>>();

        _handler = new PostAuthenticateCommandHandler(
            _userCommandRepositoryMock.Object,
            _tokenServiceMock.Object,
            _loggerMock.Object
        );
    }

    [TestMethod]
    public async Task ValidRequestReturnsToken()
    {
        var command = new PostAuthenticateCommand { Email = "test@example.com", Password = "ValidPassword" };
        var user = new User { Email = "test@example.com", Password = BCrypt.Net.BCrypt.HashPassword("ValidPassword") };

        _userCommandRepositoryMock
            .Setup(x => x.GetUserByEmailAsync(command.Email))
            .ReturnsAsync(user);

        _tokenServiceMock
            .Setup(x => x.GenerateToken(command.Email))
            .Returns("GeneratedToken");

        var result = await _handler.Handle(command, CancellationToken.None);

        IsNotNull(result);
        AreEqual("GeneratedToken", result.Token);
        _userCommandRepositoryMock.Verify(x => x.GetUserByEmailAsync(command.Email), Times.Once);
        _tokenServiceMock.Verify(x => x.GenerateToken(command.Email), Times.Once);
    }

    [TestMethod]
    public async Task UserNotFoundThrowsNotFoundException()
    {
        var command = new PostAuthenticateCommand { Email = "notfound@example.com", Password = "password" };

        _userCommandRepositoryMock
            .Setup(x => x.GetUserByEmailAsync(command.Email))
            .ReturnsAsync((User)null);

        await ThrowsExactlyAsync<NotFoundException>(async () => { await _handler.Handle(command, CancellationToken.None); });

        _userCommandRepositoryMock.Verify(x => x.GetUserByEmailAsync(command.Email), Times.Once);
        _tokenServiceMock.Verify(x => x.GenerateToken(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task InvalidPasswordThrowsBadRequestException()
    {
        var command = new PostAuthenticateCommand { Email = "test@example.com", Password = "InvalidPassword" };
        var user = new User { Email = "test@example.com", Password = BCrypt.Net.BCrypt.HashPassword("ValidPassword") };

        _userCommandRepositoryMock
            .Setup(x => x.GetUserByEmailAsync(command.Email))
            .ReturnsAsync(user);

        await ThrowsExactlyAsync<BadRequestException>(async () => { await _handler.Handle(command, CancellationToken.None); });

        _userCommandRepositoryMock.Verify(x => x.GetUserByEmailAsync(command.Email), Times.Once);
        _tokenServiceMock.Verify(x => x.GenerateToken(It.IsAny<string>()), Times.Never);
    }
}