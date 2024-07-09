using Finance.Models;
using Finance.Repository;

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
      User userAlreadyExists = new User();

      userAlreadyExists = _userRepository.SelectUserById(user.UserEmail);

      if(userAlreadyExists != null)
      {
        throw new Exception("O Usuario já existe!");
      }

      user.UserDtcad = DateOnly.FromDateTime(DateTime.Now);
      _userRepository.CreateUser(user);

      return "Usuario cadastrado com sucesso!";

    }

  }
}
