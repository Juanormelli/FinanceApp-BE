
namespace Finance.Services
{
  public class PasswdHash
  {
    public PasswdHash() { }

    public string PasswdHashGen(string passwd) 
    {

      string hash = BCrypt.Net.BCrypt.HashPassword(passwd, 11);

      return hash;

    }

    public bool VerifyPassHash(string passwd, string hash)
    {
      return BCrypt.Net.BCrypt.Verify(passwd, hash);
    }

  }
}
