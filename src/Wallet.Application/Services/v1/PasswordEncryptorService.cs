using System.Security.Cryptography;
using System.Text;
using Wallet.CrossCutting.Configuration.AppSettings;
using Wallet.Domain.Interfaces.v1.Services;

namespace Wallet.Application.Services.v1;

public class PasswordEncryptorService : IPasswordEncryptorService
{
    public string Encrypt(string password)
    {
        var newPassword = $"{password}{AppSettings.PasswordHash.DefaultHash}";

        var bytes = Encoding.UTF8.GetBytes(newPassword);
        var hashBytes = SHA512.HashData(bytes);

        return StringBytes(hashBytes);
    }

    private static string StringBytes(byte[] bytes)
    {
        var sb = new StringBuilder();

        foreach (var b in bytes)
        {
            var hex = b.ToString("x2");
            sb.Append(hex);
        }

        return sb.ToString();
    }
}