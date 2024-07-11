using Finance.Models;
using Finance.Repository;
using Finance.Services;
using Finance.UseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;

namespace Finance.Controllers
{
  public class IRequest
  {
    public string passwd;
    public string userEmail;
  }

  [Route("api/[controller]")]
  [ApiController]
  public class UserController : ControllerBase

  {
    private IUserRepository _userRepository;
    private TokenService _tokenService;

    public UserController(FinanceContext context, IDistributedCache distributed)
    {
      _userRepository = new UserRepository(context);
      _tokenService = new TokenService(distributed);
    }

    [HttpPost]
    [Route("InsertUser")]
    public IActionResult CreateUser([FromBody] User user)
    {
      try
      {
        if (!ModelState.IsValid || user == null)
        {
          return BadRequest("Dados inválidos! Tente novamente.");
        }
        else
        {
          var response = new CreateUserUseCase(_userRepository).execute(user);
          return Ok(response);
        }
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
      
    }

    [HttpPost]
    [Route("GetUserByEmail")]
    [Authorize]
    public IActionResult GetUserByEmail([FromBody] string userEmail)
    {

      try
      {
        if(userEmail == null)
        {
          return BadRequest("O Email é invalido!");
        }

        else
        {
          User user = new User();

          user = new SelectUserByIdUseCase(_userRepository).execute(userEmail);

          return Ok(user);

        }

      }
      catch (Exception ex) 
      {
        return BadRequest(ex.Message);
      }

    }
    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult<dynamic>> AuhtenticateAsync([FromBody] User usr)
    {

      try
      {
        
        var user = new AuthenticateUserUseCase(_userRepository).execute(usr.UserEmail, usr.UserPasswd);
        var token = _tokenService.GenerateToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();
        
        var tokenCache = new JwtToken();
        tokenCache.Token = refreshToken;
        tokenCache.UserId = user.UserEmail.Trim();

        _tokenService.SaveRefreshToken(tokenCache);

        return new
        {
          user = user.UserEmail,
          token = token,
          refreshToken = refreshToken,
        };


      }catch(Exception ex)
      {
        return BadRequest(ex.Message);
      }



    }
    [HttpPost]
    [Route("Refresh")]

    public async Task<dynamic> Refresh([FromBody] RefreshTokenRequest refreshTokenRequest)
    {

      var principal = _tokenService.GetPrincipalFromExpiredToken(refreshTokenRequest.Token);
      var userId = principal.Identity.Name.Trim();
      var savedRefreshToken = await _tokenService.GetRefreshToken(userId);

      if (savedRefreshToken != refreshTokenRequest.RefreshToken) 
      {
        throw new SecurityTokenException("Token Invalido");
      }

      var newToken = _tokenService.GenerateToken(principal.Claims);
      var newRefreshToken = _tokenService.GenerateRefreshToken();
      _tokenService.RemoveToken(userId);

      JwtToken jwtToken = new JwtToken();
      jwtToken.Token = newRefreshToken;
      jwtToken.UserId = userId;
      _tokenService.SaveRefreshToken(jwtToken);
      
      return new
      {
        token = newToken,
        refreshToken = newRefreshToken
      };
    }
  }
}
