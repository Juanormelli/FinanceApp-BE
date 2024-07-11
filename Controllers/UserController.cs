using Finance.Models;
using Finance.Repository;
using Finance.Services;
using Finance.UseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

    public UserController(FinanceContext context)
    {
      _userRepository = new UserRepository(context);
      _tokenService = new TokenService();
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

        return new
        {
          user = user.UserEmail,
          token = token,
        };


      }catch(Exception ex)
      {
        return BadRequest(ex.Message);
      }



    }
  }
}
