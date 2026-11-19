using OwnProject.Board.History;
using System;
using OwnProject.Board.InterFaceForB;
using OwnProject.Board.Rendering;

namespace OwnProject.Board;

public class BoardController
{
  private readonly IBoard board;
  private readonly BoardCursor boardCursor;
  private readonly Cursor brush;
  private readonly MiniBar miniBar;
  private readonly BoardRenderer renderer;
  private readonly IBoardHistory history;

  public BoardController(
      IBoard board,
      BoardCursor boardCursor,
      Cursor brush,
      BoardRenderer renderer,
      MiniBar miniBar,
      IBoardHistory history)
  {
    this.board = board;
    this.boardCursor = boardCursor;
    this.brush = brush;
    this.renderer = renderer;
    this.miniBar = miniBar;
    this.history = history;
  }

  public void Run()
  {
    bool isMiniBarFocused = false;
    bool running = true;

    try
    {
      // Первое отображение
      renderer.Render(
          boardCursor,
          miniBar: miniBar);

      while (running)
      {
        ConsoleKey key = Console.ReadKey(true).Key;

        switch (key)
        {
          case ConsoleKey.Tab:
            isMiniBarFocused = !isMiniBarFocused;
            break;

          case ConsoleKey.LeftArrow:
            if (isMiniBarFocused)
              history.Undo();
            else
              boardCursor.MoveLeft();
            break;

          case ConsoleKey.RightArrow:
            if (isMiniBarFocused)
              history.Redo();
            else
              boardCursor.MoveRight();
            break;

          case ConsoleKey.UpArrow:
            if (isMiniBarFocused)
              miniBar.SelectPreviousMenuItem();
            else
              boardCursor.MoveUp();
            break;

          case ConsoleKey.DownArrow:
            if (isMiniBarFocused)
              miniBar.SelectNextMenuItem();
            else
              boardCursor.MoveDown();
            break;

          case ConsoleKey.Spacebar:
            if (!isMiniBarFocused)
            {
              history.SaveState();

              board.SetCell(
                  boardCursor.X,
                  boardCursor.Y,
                  new BoardCell
                  {
                    Symbol = brush.Symbol,
                    Color = brush.Color
                  });
            }
            break;

          case ConsoleKey.Escape:
            running = false;
            break;
        }

        // Обновляем и холст, и панель
        if (running)
        {
          renderer.Render(
              boardCursor,
              miniBar: miniBar);
        }
      }
    }
    finally
    {
      Console.ResetColor();
    }
  }
}
