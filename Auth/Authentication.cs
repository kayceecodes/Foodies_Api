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
        string publicKey = _config["PublicKey"];
        var key = Encoding.UTF8.GetBytes(publicKey);
        var sKey = new SymmetricSecurityKey(key);
        var SignedCredentials = new SigningCredentials(sKey, SecurityAlgorithms.HmacSha256Signature);

        var claims = new ClaimsIdentity(new []
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        });
        var expires = DateTime.UtcNow.AddHours(1);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = expires,
            Issuer = "MasterBlazor",
            SigningCredentials = SignedCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenJwt = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(tokenJwt);

        return token;
    }
}
