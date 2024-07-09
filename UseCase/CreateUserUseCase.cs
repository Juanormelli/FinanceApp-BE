using Finance.Models;
using Finance.Repository;
using Finance.Services;

namespace Finance.UseCase
{
  public class CreateUserUseCase
  {
    IUserRepository _userRepository;

    public CreateUserUseCase(IUserRepository userRepository)
    {

      _userRepository = userRepository;

    }

    public string execute(User user)
    {
      PasswdHash passwd = new PasswdHash();
      User userAlreadyExists = new User();

      userAlreadyExists = _userRepository.SelectUserById(user.UserEmail);

      if(userAlreadyExists != null)
      {
        throw new Exception("O Usuario já existe!");
      }


      user.UserPasswd = passwd.PasswdHashGen(user.UserPasswd);
      user.UserDtcad = DateOnly.FromDateTime(DateTime.Now);
      _userRepository.CreateUser(user);

      return "Usuario cadastrado com sucesso!";

    }

  }
}
