
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
namespace TimeCardCore.Infrastructure;
public interface IJWTokenAuthentication
{
    string GenerateToken(int id, string name, int expiresSeconds);
    bool ValidateToken(string token, int skewMinutes = 0);
}
public class JWTokenAuthentication : IJWTokenAuthentication
{
    public readonly string _JWTokenSecret;
    public JWTokenAuthentication(IConfiguration config)
    {
        _JWTokenSecret = GetTokenSecret(config);
    }
    private string GetTokenSecret(IConfiguration config)
    {
        var mySecret = config.GetValue<string>("JWTokenSecret");
        if (mySecret == "SECRET")
        {
            throw new Exception("JWTokenSecret not configured");
        }
        if (mySecret.Length <= 256)
        {
            mySecret += mySecret; //make it large enough
        }
        return mySecret;
    }
    public string GenerateToken(int id, string name, int expiresSeconds)
    {
        var mySecret = _JWTokenSecret;

        var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));
        var myIssuer = "Greg Corron";
        var myAudience = "FWSI";
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
        new Claim("id", id.ToString()),
        new Claim("name", name)
            }),
            Expires = DateTime.UtcNow.AddSeconds(expiresSeconds),
            Issuer = myIssuer,
            Audience = myAudience,
            SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    public bool ValidateToken(string token, int skewMinutes)
    {
        return DecodeToken(token, skewMinutes) != null;
    }
#nullable enable
    public Tuple<int, string>? DecodeToken(string token, int skewMinutes = 0)
    {
        int id = 0;
        string name = "";
        var mySecret = _JWTokenSecret;
        var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));
        var myIssuer = "Greg Corron";
        var myAudience = "FWSI";
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = myIssuer,
                ValidAudience = myAudience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(skewMinutes),
                IssuerSigningKey = mySecurityKey
            }, out SecurityToken validatedToken);
        }
        catch
        {
            return null;
        }
        var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
        var idString = securityToken.Claims.First(claim => claim.Type == "id").Value;
        int.TryParse(idString, out id);
        name = securityToken.Claims.First(claim => claim.Type == "name").Value;
        return new Tuple<int, string>(id, name);
    }
}


