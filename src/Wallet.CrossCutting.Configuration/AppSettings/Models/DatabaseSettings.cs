namespace Wallet.CrossCutting.Configuration.AppSettings.Models;

public sealed class DatabaseSettings
{
    public string Host { get; init; } = null!;
    public string Base { get; init; } = null!;
    public string User { get; init; } = null!;
    public string Password { get; init; } = null!;
}