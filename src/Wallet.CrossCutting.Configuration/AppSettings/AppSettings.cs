using Wallet.CrossCutting.Configuration.AppSettings.Models;

namespace Wallet.CrossCutting.Configuration.AppSettings;

public static class AppSettings
{
    public static void Initialize(
        DatabaseSettings databaseSettings, 
        JwtSettings jwtSettings)
    {
        Database = databaseSettings;
        Jwt = jwtSettings;
    }
    
    public static DatabaseSettings Database { get; private set; } = null!;
    public static JwtSettings Jwt { get; private set; } = null!;
}