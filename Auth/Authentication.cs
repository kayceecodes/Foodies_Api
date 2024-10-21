using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using foodies_api.Models;
using Microsoft.IdentityModel.Tokens;

namespace foodies_api.Auth;

public class Authentication(IConfiguration config)
{
    private readonly IConfiguration _config = config;

    public string CreateAccessToken(User user)
    {
        string publicKey = _config["JwtSettings:Key"];
        var key = Encoding.UTF8.GetBytes(publicKey);
        var sKey = new SymmetricSecurityKey(key);
        var SignedCredentials = new SigningCredentials(sKey, SecurityAlgorithms.HmacSha256Signature);

        var claims = new ClaimsIdentity(new []
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(ClaimTypes.Name, user.FirstAndLastName),
        });
        
        var expires = DateTime.UtcNow.AddMonths(1);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = expires,
            Issuer = "Kayceecode",
            Audience = _config["JwtSettings:Audience"],
            SigningCredentials = SignedCredentials,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenJwt = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(tokenJwt);

        return token;
    }
}
