namespace Wallet.Domain.Interfaces.v1.Services;

public interface ITokenService
{
    string GenerateToken(string username);
}