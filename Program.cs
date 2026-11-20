using System;
using System.Text;

using OwnProject;
using OwnProject.Files;
using OwnProject.REGANDAUT;
using OwnProject.Board;
using OwnProject.Board.History;
using OwnProject.Board.InterFaceForB;
using OwnProject.Board.Rendering;

namespace OwnProject;

internal static class Program
{
  private static void Main()
  {
    bool loggerStarted = false;
    AuthenticationService? authentication = null;
    User? currentUser = null;

    try
    {
      Console.InputEncoding = Encoding.UTF8;
      Console.OutputEncoding = Encoding.UTF8;

      AppLogger.Init();
      loggerStarted = true;

      AppPaths.EnsureCreated();

      // Создаём авторизацию
      IFileRepository repository = new JsonFileRepository();
      authentication = new AuthenticationService(repository);

      // Показываем меню регистрации и входа
      AuthenticationMenu authMenu =
          new AuthenticationMenu(authentication);



      Console.WriteLine("AUTHENTICATION MENU STARTED");
      Console.ReadKey(true);

      currentUser = authMenu.Run();

      Console.WriteLine(
          currentUser == null
              ? "USER IS NULL"
              : $"USER LOGGED IN: {currentUser.Login}"
      );

      Console.ReadKey(true);

      // Выход без входа
      if (currentUser == null)
        return;

      // Создаём пустой холст
      int width = Math.Clamp(Console.WindowWidth - 28, 20, 60);
      int height = Math.Clamp(Console.WindowHeight - 4, 12, 20);

      IBoard board = new OwnBoard(width, height);
      BoardCursor boardCursor = new BoardCursor(board);
      Cursor brush = new Cursor();

      MiniBar miniBar = new MiniBar(board, BarSide.Right);
      BoardRenderer renderer = new BoardRenderer(board);
      IBoardHistory history = new BoardHistory(board);

      // Запускаем редактор
      BoardController controller = new BoardController(
          board,
          boardCursor,
          brush,
          renderer,
          miniBar,
          history
      );

      controller.Run();
    }
    catch (Exception ex)
    {
      Console.ResetColor();
      Console.WriteLine($"Application error: {ex}");

      if (loggerStarted)
        AppLogger.Fatal($"Application error: {ex}");
    }
    finally
    {
      if (currentUser != null && authentication != null)
        authentication.Logout(currentUser);

      if (loggerStarted)
        AppLogger.Shutdown();

      Console.ResetColor();
    }
  }
}
