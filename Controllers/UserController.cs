using Finance.Models;
using Finance.Repository;
using Finance.UseCase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Finance.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UserController : ControllerBase

  {
    private IUserRepository _userRepository;

    public UserController(FinanceContext context)
    {
      _userRepository = new UserRepository(context);
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
  }
}
