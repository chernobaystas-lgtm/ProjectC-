using System;
using OwnProject.Board;
using OwnProject.Board.InterFaceForB;

namespace OwnProject;

public class OwnBoard : IBoard
{
  private BoardCell[,] cells;

  public int Width { get; }
  public int Height { get; }

  public OwnBoard(int width, int height)
  {
    if (width <= 0)
      throw new ArgumentOutOfRangeException(nameof(width));

    if (height <= 0)
      throw new ArgumentOutOfRangeException(nameof(height));

    Width = width;
    Height = height;

    // Создаём двумерный массив
    cells = new BoardCell[height, width];

    // Заполняем доску пустыми клетками
    for (int y = 0; y < Height; y++)
    {
      for (int x = 0; x < Width; x++)
      {
        cells[y, x] = new BoardCell
        {
          Symbol = ' ',
          Color = ConsoleColor.White
        };
      }
    }
  }

  // Получение содержимого клетки
  public BoardCell GetCell(int x, int y)
  {
    CheckCoordinates(x, y);

    return cells[y, x];
  }

  // Изменение содержимого клетки
  public void SetCell(int x, int y, BoardCell cell)
  {
    CheckCoordinates(x, y);

    cells[y, x] = cell;
  }

  // Проверка координат
  private void CheckCoordinates(int x, int y)
  {
    if (x < 0 || x >= Width)
      throw new ArgumentOutOfRangeException(nameof(x));

    if (y < 0 || y >= Height)
      throw new ArgumentOutOfRangeException(nameof(y));
  }
}
