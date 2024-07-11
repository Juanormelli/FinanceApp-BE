using Finance.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Finance.Services
{
  public class TokenService
  {
    IDistributedCache _distributed;
    public TokenService(IDistributedCache distributedCache) 
    {
      _distributed = distributedCache;
    }

    public string GenerateToken(User user)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("SECRET"));

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new System.Security.Claims.ClaimsIdentity(new[]
        {
          new Claim(ClaimTypes.Name, user.UserEmail)
        }),
        Expires = DateTime.UtcNow.AddDays(1),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };

      var token = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token);


    }

    public string GenerateToken(IEnumerable<Claim> claims)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("SECRET"));

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddDays(1),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
      var randomNumber = new byte[32];
      using var rng = RandomNumberGenerator.Create();
      rng.GetBytes(randomNumber);
      return Convert.ToBase64String(randomNumber);

    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
      var tokenValidationParameters = new TokenValidationParameters
      {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"))),
        ValidateLifetime = false
      };

      var tokenHandler = new JwtSecurityTokenHandler();
      var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
      if(securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
      {
        throw new SecurityTokenException("Invalid Token");
      }
      return principal;
    }

    public async void SaveRefreshToken(JwtToken token)
    {
    
      await _distributed.SetStringAsync(token.UserId, token.Token);
    }

    public async Task<string> GetRefreshToken(string userId)
    {

      var refreshToken =System.Text.Encoding.UTF8.GetString(await _distributed.GetAsync(userId));
      if (refreshToken == null)
      {
        return null;
      }
      return refreshToken;
      
    }

    public async void RemoveToken(string userId)
    {
      await _distributed.RemoveAsync(userId);
    }
  }
}
