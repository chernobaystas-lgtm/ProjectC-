
using OwnProject;
using OwnProject.Files;

namespace OwnProject.REGANDAUT;

public class AuthenticationService
{
  private readonly IFileRepository _repository;
  private readonly string _usersPath;

  public AuthenticationService(IFileRepository repository)
  {
    _repository = repository;
    _usersPath = AppPaths.UsersFile;
  }

  public User? Register(string login, string password, string? email = null)
  {
    var users = _repository.GetAll<User>(_usersPath);

    if (users.Any(u => u.Login == login))
      return null;

    int id = users.Count == 0 ? 1 : users.Max(u => u.Id) + 1;

    var user = new User(login, password, id, email);

    users.Add(user);
    _repository.Save(_usersPath, users);

    AppLogger.Information($"Registered: {login}");
    return user;
  }

  public User? Login(string login, string password)
  {
    var users = _repository.GetAll<User>(_usersPath);
    var user = users.FirstOrDefault(u => u.Login == login);

    if (user == null || !user.VerifyPassword(password))
    {
      AppLogger.Warning($"Failed login: {login}");
      return null;
    }

    AppLogger.Information($"Login: {login}");
    return user;
  }

  public void Logout(User user)
  {
    AppLogger.Information($"Logout: {user.Login}");
  }
}
