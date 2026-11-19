
namespace OwnProject.Board;

using System;
using System.Collections.Generic;
using OwnProject.Board.InterFaceForB;
using OwnProject.Board.Methods;
using OwnProject.Board.Tools;

public enum BarSide
{
  Left,
  Right,
  Top,
  Bottom
}

public class MiniBar
{
  private readonly List<IDrawingTool> tools = new();

  private int selectedIndex = 0;

  // Расположение панели
  public BarSide Side { get; }

  // Координаты панели
  public int X { get; }
  public int Y { get; }

  public (int X, int Y) Position => (X, Y);

  // Размеры панели
  public int Width { get; }
  public int Height { get; }

  // Список инструментов
  public IReadOnlyList<IDrawingTool> Tools => tools.AsReadOnly();

  // Индекс выбранного инструмента
  public int SelectedIndex => selectedIndex;

  // Активный инструмент
  public IDrawingTool ActiveTool => tools.Count > 0
      ? tools[selectedIndex]
      : throw new InvalidOperationException(
          "Панель не содержит инструментов.");

  public MiniBar(
      IBoard board,
      BarSide side = BarSide.Right,
      int gap = 2)
  {
    ArgumentNullException.ThrowIfNull(board);

    if (gap < 0)
      throw new ArgumentOutOfRangeException(nameof(gap));

    if (side != BarSide.Right)
      throw new NotSupportedException(
          "Пока поддерживается только правая сторона.");

    Side = side;

    // Ширина — примерно треть доски
    Width = Math.Min(
        board.Width,
        Math.Max(18, board.Width / 3));

    // Высота — примерно 80% высоты доски
    Height = Math.Min(
        board.Height,
        Math.Max(14, (int)Math.Ceiling(board.Height * 0.8)));

    // Положение справа и по центру
    X = board.Width + 2 + gap;

    Y = 1 + Math.Max(
        0,
        (board.Height - Height - 2) / 2);

    // Регистрируем инструменты
    AddTool(new BrushTool());
    AddTool(new EraserTool());
    AddTool(new LineTool());
    AddTool(new RectangleTool());
    AddTool(new TriangleTool());
    AddTool(new EllipseTool());
  }

  // Добавление инструмента
  public void AddTool(IDrawingTool tool)
  {
    ArgumentNullException.ThrowIfNull(tool);

    if (string.IsNullOrWhiteSpace(tool.Name))
      throw new ArgumentException(
          "У инструмента должно быть название.");

    bool exists = tools.Exists(t =>
        string.Equals(
            t.Name,
            tool.Name,
            StringComparison.OrdinalIgnoreCase));

    if (exists)
      throw new InvalidOperationException(
          "Такой инструмент уже существует.");

    tools.Add(tool);
  }

  // Выбор по индексу
  public bool SelectTool(int index)
  {
    if (index < 0 || index >= tools.Count)
      return false;

    selectedIndex = index;
    return true;
  }

  // Выбор по названию
  public bool SelectTool(string name)
  {
    if (string.IsNullOrWhiteSpace(name))
      return false;

    int index = tools.FindIndex(t =>
        string.Equals(
            t.Name,
            name,
            StringComparison.OrdinalIgnoreCase));

    return SelectTool(index);
  }

  // Следующий инструмент
  public void SelectNext()
  {
    if (tools.Count == 0)
      return;

    selectedIndex = (selectedIndex + 1) % tools.Count;
  }

  // Предыдущий инструмент
  public void SelectPrevious()
  {
    if (tools.Count == 0)
      return;

    selectedIndex =
        (selectedIndex - 1 + tools.Count) % tools.Count;
  }



  private static readonly IReadOnlyList<string> menuItems =
      Array.AsReadOnly(new[]
      {
        "Рисовать",
        "Стирать",
        "Фигуры и линия",
        "Цвет",
        "Символ",
        "Размер"
      });

  private int selectedMenuIndex = 0;

  public IReadOnlyList<string> MenuItems => menuItems;

  public int SelectedMenuIndex => selectedMenuIndex;

  // Переключение выбранного раздела
  public void SelectNextMenuItem()
  {
    selectedMenuIndex =
        (selectedMenuIndex + 1) % menuItems.Count;
  }

  public void SelectPreviousMenuItem()
  {
    selectedMenuIndex =
        (selectedMenuIndex - 1 + menuItems.Count)
        % menuItems.Count;
  }

  public bool SelectMenuItem(int index)
  {
    if (index < 0 || index >= menuItems.Count)
      return false;

    selectedMenuIndex = index;
    return true;
  }
}