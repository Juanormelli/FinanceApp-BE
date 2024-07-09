using Finance.Models;

namespace Finance.Repository
{
  public class UserRepository : IUserRepository
  {
    // Injeção de dependencia
    private  FinanceContext _context;
    public UserRepository(FinanceContext context) 
    {
      _context = context;
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

  }
}
