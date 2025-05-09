namespace Wallet.CrossCutting.Configuration.AppSettings.Models;

public sealed record PasswordHashSettings
{
    public string DefaultHash { get; init; } = null!;
}