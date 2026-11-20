
namespace OwnProject.Files;

public static class AppPaths
{
  public static string RootDirectory =>
      Path.Combine(
          Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
          "OwnProject");

  public static string UsersFile =>
      Path.Combine(RootDirectory, "users.json");

  public static string LogsDirectory =>
      Path.Combine(RootDirectory, "Logs");

  public static void EnsureCreated()
  {
    Directory.CreateDirectory(RootDirectory);
    Directory.CreateDirectory(LogsDirectory);
  }
}

