namespace Wallet.Domain.Interfaces.v1.Services;

public interface IPasswordEncryptorService
{
    string Encrypt(string password);
}