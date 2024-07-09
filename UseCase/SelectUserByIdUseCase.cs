using Finance.Models;
using Finance.Repository;

namespace Finance.UseCase
{
  public class SelectUserByIdUseCase
  {
    IUserRepository _userRepository;
    public SelectUserByIdUseCase( IUserRepository userRepository) 
    {

      _userRepository = userRepository;

    }

    public User execute(string email)
    {
      User user = new User();

      user = _userRepository.SelectUserById(email);
      if(user != null)
        return user;
     
     throw new Exception("O Usuario não existe");

    }
  }
}
