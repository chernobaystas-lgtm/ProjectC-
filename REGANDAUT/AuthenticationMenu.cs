
using OwnProject;

namespace OwnProject.REGANDAUT;

public class AuthenticationMenu
{
  private readonly AuthenticationService _authentication;

  public AuthenticationMenu(AuthenticationService authentication)
  {
    _authentication = authentication;
  }

  public User? Run()
  {
    while (true)
    {
      Console.Clear();
      Console.WriteLine("===== OWN PROJECT =====");
      Console.WriteLine("1. Login");
      Console.WriteLine("2. Register");
      Console.WriteLine("0. Exit");
      Console.Write("Choose: ");

      string? choice = Console.ReadLine();

      switch (choice)
      {
        case "1":
          if (LoginMenu() is User loggedUser)
            return loggedUser;
          break;

        case "2":
          RegisterMenu();
          break;

        case "0":
          return null;

        default:
          Console.WriteLine("Invalid choice.");
          Console.ReadKey();
          break;
      }
    }
  }

  private User? LoginMenu()
  {
    Console.Clear();
    Console.Write("Login: ");
    string login = Console.ReadLine() ?? "";

    Console.Write("Password: ");
    string password = Console.ReadLine() ?? "";

    var user = _authentication.Login(login, password);

    Console.WriteLine(user == null
        ? "Invalid login or password."
        : $"Welcome, {user.Login}!");

    Console.ReadKey();
    return user;
  }

  private void RegisterMenu()
  {
    Console.Clear();

    Console.Write("Login: ");
    string login = Console.ReadLine() ?? "";

    Console.Write("Password: ");
    string password = Console.ReadLine() ?? "";

    Console.Write("Email: ");
    string email = Console.ReadLine() ?? "";

    var user = _authentication.Register(login, password, email);

    Console.WriteLine(user == null
        ? "This login is already taken."
        : "Registration successful!");

    Console.ReadKey();
  }
}
