
using System;
using OwnProject.Board.History;
using OwnProject.Board.InterFaceForB;
using OwnProject.Board.Rendering;
using OwnProject.Board.Tools;

namespace OwnProject.Board;

public class BoardController
{
  private readonly IBoard board;
  private readonly BoardCursor boardCursor;
  private readonly Cursor brush;
  private readonly MiniBar miniBar;
  private readonly BoardRenderer renderer;
  private readonly IBoardHistory history;

  private static readonly ConsoleColor[] Colors =
  {
        ConsoleColor.White,
        ConsoleColor.Red,
        ConsoleColor.Yellow,
        ConsoleColor.Green,
        ConsoleColor.Cyan,
        ConsoleColor.Blue,
        ConsoleColor.Magenta
    };

  private static readonly char[] Symbols =
  {
        '#', '*', '+', '@', '%', '&', 'X', 'O'
    };

  private int colorIndex;
  private int symbolIndex;
  private (int X, int Y)? shapeStart;

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

    bool panelFocused = false;
    bool running = true;

    try
    {
      renderer.Render(boardCursor, miniBar: miniBar);

      while (running)
      {
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        ConsoleKey key = keyInfo.Key;

        if (keyInfo.KeyChar == (char)124 ||
            keyInfo.KeyChar == (char)92)
        {
          ActivateMenuItem();
          panelFocused = false;
        }
        else if (keyInfo.KeyChar == '-' || keyInfo.KeyChar == '_')
        {
          miniBar.SelectTool(1);
          shapeStart = null;
          panelFocused = false;
        }
        else if (keyInfo.KeyChar == '+' || keyInfo.KeyChar == '=')
        {
          miniBar.SelectTool(0);
          shapeStart = null;
          panelFocused = false;
        }
        else
        {
          switch (key)
          {
            case ConsoleKey.Tab:
              panelFocused = !panelFocused;
              break;

            case ConsoleKey.LeftArrow:
              if (panelFocused)
                history.Undo();
              else
                boardCursor.MoveLeft();
              break;

            case ConsoleKey.RightArrow:
              if (panelFocused)
                history.Redo();
              else
                boardCursor.MoveRight();
              break;

            case ConsoleKey.UpArrow:
              if (panelFocused)
                miniBar.SelectPreviousMenuItem();
              else
                boardCursor.MoveUp();
              break;

            case ConsoleKey.DownArrow:
              if (panelFocused)
                miniBar.SelectNextMenuItem();
              else
                boardCursor.MoveDown();
              break;

            case ConsoleKey.Enter:
              if (panelFocused)
              {
                ActivateMenuItem();
              }
              break;

            case ConsoleKey.Spacebar:
              if (!panelFocused)
                Draw();
              break;

            case ConsoleKey.Backspace:
              miniBar.SelectTool(1);
              shapeStart = null;
              panelFocused = false;
              break;

            case ConsoleKey.Escape:
              running = false;
              break;
          }
        }

        if (running)
          renderer.Render(boardCursor, miniBar: miniBar);
      }
    }
    finally
    {
      Console.ResetColor();
    }
  }


  private void ActivateMenuItem()
  {
    switch (miniBar.SelectedMenuIndex)
    {
      case 0: // 1. Рисовать
        miniBar.SelectTool(0);
        shapeStart = null;
        break;

      case 1: // 2. Стирать
        miniBar.SelectTool(1);
        shapeStart = null;
        break;

      case 2: // 3. Фигуры и линия
        SelectNextFigure();
        break;

      case 3: // 4. Цвет
        ChangeColor();
        break;

      case 4: // 5. Символ
        ChangeSymbol();
        break;

      case 5: // 6. Размер
        ChangeSize();
        break;
    }
  }

  private void SelectNextFigure()
  {
    string[] figureTools =
    {
            "Line",
            "Rectangle",
            "Triangle",
            "Ellipse"
        };

    int current = Array.IndexOf(
        figureTools,
        miniBar.ActiveTool.Name);

    int next = (current + 1) % figureTools.Length;

    miniBar.SelectTool(figureTools[next]);
    shapeStart = null;
  }

  private void ChangeColor()
  {
    colorIndex = (colorIndex + 1) % Colors.Length;
    brush.ChangeColor(Colors[colorIndex]);
  }

  private void ChangeSymbol()
  {
    symbolIndex = (symbolIndex + 1) % Symbols.Length;
    brush.ChangeSymbol(Symbols[symbolIndex]);
  }

  private void ChangeSize()
  {
    Console.Write("Ширина кисти (1-20): ");

    if (!int.TryParse(Console.ReadLine(), out int width))
      return;

    Console.Write("Высота кисти (1-20): ");

    if (!int.TryParse(Console.ReadLine(), out int height))
      return;

    try
    {
      brush.ChangeSize(width, height);
      Console.WriteLine($"Размер кисти: {width} x {height}");
    }
    catch (ArgumentOutOfRangeException)
    {
      Console.WriteLine("Размер должен быть от 1 до 20.");
    }
  }

  private void Draw()
  {
    string toolName = miniBar.ActiveTool.Name;

    BoardCell cell = new BoardCell
    {
      Symbol = brush.Symbol,
      Color = brush.Color
    };

    // Кисть и ластик работают сразу.
    if (toolName == "Brush" || toolName == "Eraser")
    {
      shapeStart = null;
      history.SaveState();

      miniBar.ActiveTool.Execute(
          board,
          boardCursor.X,
          boardCursor.Y,
          boardCursor.X,
          boardCursor.Y,
          cell,
          brush.Width,
          brush.Height);

      return;
    }

    // Первое нажатие запоминает начальную точку.
    if (shapeStart == null)
    {
      shapeStart = (boardCursor.X, boardCursor.Y);
      return;
    }

    // Второе нажатие рисует от первой точки до текущей.
    var start = shapeStart.Value;

    history.SaveState();

    miniBar.ActiveTool.Execute(
        board,
        start.X,
        start.Y,
        boardCursor.X,
        boardCursor.Y,
        cell,
        brush.Width,
        brush.Height);

    shapeStart = null;
  }

}
