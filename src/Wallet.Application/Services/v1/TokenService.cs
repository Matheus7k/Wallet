using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Wallet.CrossCutting.Configuration.AppSettings;
using Wallet.Domain.Interfaces.v1;
using Wallet.Domain.Interfaces.v1.Services;

namespace Wallet.Application.Services.v1;

public sealed class TokenService : ITokenService
{
    public string GenerateToken(string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var key = Encoding.ASCII.GetBytes(AppSettings.Jwt.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.Name, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())]),
            Expires = DateTime.UtcNow.AddMinutes(AppSettings.Jwt.ExpirationMinutes),
            Issuer = AppSettings.Jwt.Issuer,
            Audience = AppSettings.Jwt.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}