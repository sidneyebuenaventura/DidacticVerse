using DidacticVerse.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace DidacticVerse.Accounts;

public class AccountIdentity
{
    private string _issuer { get; set; }
    private string _audience { get; set; }
    private byte[] _key { get; set; }

    public AccountIdentity(IConfiguration configuration)
    {
        _issuer = configuration["Jwt:Issuer"];
        _audience = configuration["Jwt:Audience"];
        _key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
    }

    public SecurityTokenDescriptor TokenDescriptor(long accountId) {
        return new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Name, accountId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(45),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(_key),
                SecurityAlgorithms.HmacSha512Signature
            )
        };
    }
    public SecurityTokenDescriptor ResetTokenDescriptor(RefreshTokens refresh)
    {


        return new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", refresh.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, refresh.AccountId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(60),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(_key), 
                SecurityAlgorithms.HmacSha256Signature
            )
        };
    }
}
