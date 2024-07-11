using Finance.Models;
using Finance.Repository;
using Finance.Services;
using Finance.UseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;

namespace Finance.Controllers
{

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
        Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
        Response.Cookies.Append("X-Username", usr.UserEmail.Trim(), new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
        Response.Cookies.Append("X-Refresh-Token", refreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
        return Ok();


      }catch(Exception ex)
      {
        return BadRequest(ex.Message);
      }



    }
    [HttpPost]
    [Route("Refresh")]

    public async Task<dynamic> Refresh()
    {

      var principal = _tokenService.GetPrincipalFromExpiredToken(Request.Cookies["X-Access-Token"]);
      var exp = principal.Claims.Where(x => x.Type == "exp").FirstOrDefault();
      var userId = principal.Identity.Name.Trim();
      var savedRefreshToken = await _tokenService.GetRefreshToken(userId);

      if (savedRefreshToken != Request.Cookies["X-Refresh-Token"]) 
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

      Response.Cookies.Append("X-Access-Token", newToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
      Response.Cookies.Append("X-Username", userId, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
      Response.Cookies.Append("X-Refresh-Token", newRefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

      return Ok();

    }
  }
}
