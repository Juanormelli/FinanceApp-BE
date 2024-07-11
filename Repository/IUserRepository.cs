using Finance.Models;

namespace Finance.Repository
{
  public interface IUserRepository
  {
    public string CreateUser(User users);
    public User SelectUserById(string email);

    public User ValidateLogin(string email, string passwd);
  }
}
