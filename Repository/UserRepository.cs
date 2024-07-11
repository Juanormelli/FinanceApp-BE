using Finance.Models;
using Finance.Services;
using System.Runtime.CompilerServices;

namespace Finance.Repository
{
  public class UserRepository : IUserRepository
  {
    // Injeção de dependencia
    private  FinanceContext _context;
    private PasswdHash _passwdHash;
    public UserRepository(FinanceContext context) 
    {
      _context = context;
      _passwdHash = new PasswdHash();
    }

    public string CreateUser(User users)
    {
      _context.Add(users);
      _context.SaveChanges();

      return "Usuario criado com sucesso";
    }
    public User SelectUserById(string email)
    {
      User user = new User();

      user = _context.Users.FirstOrDefault(x => x.UserEmail == email);

      if (user == null)
      {
        return null;
      }

      return user;
    }
    public User ValidateLogin(string email, string passwd)
    {
      User user = new User();


      user = _context.Users.FirstOrDefault(x => x.UserEmail == email);

     

      if (user == null)
      {
        return null;
      }

      bool validatePasswd = _passwdHash.VerifyPassHash(passwd, user.UserPasswd);
      
      if(validatePasswd)
      {
        return user;
      }

      return null;

    }
  }
}
