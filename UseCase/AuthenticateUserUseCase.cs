using Finance.Models;
using Finance.Repository;
using Finance.Services;

namespace Finance.UseCase
{
  public class AuthenticateUserUseCase
  {
    IUserRepository _userRepository;

    public AuthenticateUserUseCase(IUserRepository userRepository)
    {

      _userRepository = userRepository;

    }

    public User execute(string email, string passwd)
    {
      PasswdHash password = new PasswdHash();
      
      User user = _userRepository.ValidateLogin(email, passwd);



      if (user == null)
      {
        throw new Exception("Usuario ou senha incorreto!");
      }

      return user;

    }
  }
}
