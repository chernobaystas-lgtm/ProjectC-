
namespace OwnProject.Files;

public static class AppLogger
{
  private static readonly object LockObject = new();
  private static string LogFile =>
      Path.Combine(AppPaths.LogsDirectory, "app.log");

  public static void Init()
  {
    AppPaths.EnsureCreated();
    Information("Application started");
  }

  public static void Verbose(string message) => Write("VERBOSE", message);
  public static void Debug(string message) => Write("DEBUG", message);
  public static void Information(string message) => Write("INFO", message);
  public static void Warning(string message) => Write("WARNING", message);
  public static void Error(string message) => Write("ERROR", message);
  public static void Fatal(string message) => Write("FATAL", message);

  public static void Error(Exception ex) =>
      Write("ERROR", ex.ToString());

  public static void Fatal(Exception ex) =>
      Write("FATAL", ex.ToString());

  public static void Shutdown()
  {
    Information("Application stopped");
  }

  private static void Write(string level, string message)
  {
    try
    {
      AppPaths.EnsureCreated();

      lock (LockObject)
      {
        File.AppendAllText(
            LogFile,
            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}{Environment.NewLine}");
      }
    }
    catch
    {
      // Ошибка логирования не должна останавливать приложение.
    }
  }
}
