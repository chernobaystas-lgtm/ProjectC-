namespace OwnProject;

public class User
{
  public string Login { get; set; } = string.Empty;
  public int Id { get; set; } = 0;
  public string PasswordHash { get; set; } = string.Empty;
  public string? Email { get; set; }

  public List<Drawing> Drawings { get; set; } = new List<Drawing>();
  public List<LoginLog> LoginHistory { get; set; } = new List<LoginLog>();

  public User() { }

  public User(string login)
  {
    Login = login;
    Id = GenerateId();
  }

  public User(string login, string password)
  {
    Login = login;
    PasswordHash = HashPassword(password);
    Id = GenerateId();
  }

  public User(string login, string password, int id)
  {
    Login = login;
    PasswordHash = HashPassword(password);
    Id = id;
  }


  public User(string login, string password, int id, string? email = null)
  {
    if (string.IsNullOrWhiteSpace(login))
      throw new ArgumentException("Login cannot be empty");

    if (string.IsNullOrWhiteSpace(password))
      throw new ArgumentException("Password cannot be empty");

    if (id <= 0)
      throw new ArgumentOutOfRangeException(nameof(id));

    Login = login;
    PasswordHash = HashPassword(password);
    Id = id;
    Email = email;
  }

  private static int GenerateId()
  {
    return Random.Shared.Next(1000, 9999);
  }

  public static string HashPassword(string password)
  {
    using (var sha = System.Security.Cryptography.SHA256.Create())
    {
      var bytes = System.Text.Encoding.UTF8.GetBytes(password);
      var hash = sha.ComputeHash(bytes);
      return Convert.ToBase64String(hash);
    }
  }

  public bool VerifyPassword(string password)
  {
    return PasswordHash == HashPassword(password);
  }

  public override string ToString()
  {
    return $"{Login} (ID: {Id})";
  }

  public override bool Equals(object? obj)
  {
    if (obj is not User other)
      return false;

    return Id == other.Id;
  }

  public override int GetHashCode()
  {
    return Id.GetHashCode();
  }
}