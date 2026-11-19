// konsole -e dotnet run чтоб в консоле все выводилось

using System.Text;
using OwnProject;
using OwnProject.Board;
using OwnProject.Board.InterFaceForB;
using OwnProject.Board.Rendering;
using OwnProject.Board.Tools;
using OwnProject.Board.History;

namespace Manager;

internal class Program
{
  private static void Main(string[] args)
  {
    Console.OutputEncoding = Encoding.UTF8;

    Console.Clear();

    IBoard board = new OwnBoard(60, 20);

    BoardCursor boardCursor = new BoardCursor(board);

    Cursor brush = new Cursor();

    MiniBar miniBar = new MiniBar(
        board,
        BarSide.Right);

    BoardRenderer renderer = new BoardRenderer(board);
    IBoardHistory history = new BoardHistory(board);

    BoardController controller = new BoardController(
        board,
        boardCursor,
        brush,
        renderer,
        miniBar,
        history);

    // Запускаем основной цикл приложения
    controller.Run();
  }
}
