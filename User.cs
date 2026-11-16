using System;
using static System.Console;
namespace OwnProject;

public class User
{
  private string _login { get; init; } = string.Empty;
  private string _password { get; init; } = string.Empty;
  private int _id { get; init; } = 0;

  private string _email { get; init; } = string.Empty;

  public User() { }


  public User(string login, string password, int id, string email)
  {
    _login = login;
    _password = password;
    _id = id;
    _email = email;
  }

  ~User()
  {

  }


}
