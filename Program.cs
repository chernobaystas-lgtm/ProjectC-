using static System.Console;
// konsole -e dotnet run чтоб в консоле все выводилось

namespace Manager;


internal class Program
{
  private static void Main(string[] args)
  {

    AppLogger.Init();
    WriteLine("Hello, World!");
    ReadKey();
    AppLogger.Shutdown();

  }
}