
namespace OwnProject.Board.Tools;

using System;
using OwnProject.Board;
using OwnProject.Board.InterFaceForB;

public static class LineRasterizer
{
  public static void DrawLine(
      IBoard board,
      int startX,
      int startY,
      int endX,
      int endY,
      BoardCell cell,
      int thickness)
  {
    ArgumentNullException.ThrowIfNull(board);

    if (thickness <= 0)
      throw new ArgumentOutOfRangeException(nameof(thickness));

    CheckCoordinates(board, startX, startY);
    CheckCoordinates(board, endX, endY);

    int x = startX;
    int y = startY;

    int dx = Math.Abs(endX - startX);
    int dy = Math.Abs(endY - startY);

    int sx = startX < endX ? 1 : -1;
    int sy = startY < endY ? 1 : -1;

    int error = dx - dy;

    while (true)
    {
      DrawThickPoint(board, x, y, cell, thickness);

      if (x == endX && y == endY)
        break;

      int error2 = 2 * error;

      if (error2 > -dy)
      {
        error -= dy;
        x += sx;
      }

      if (error2 < dx)
      {
        error += dx;
        y += sy;
      }
    }
  }

  // Рисование точки с учётом толщины
  private static void DrawThickPoint(
      IBoard board,
      int x,
      int y,
      BoardCell cell,
      int thickness)
  {
    int offset = -(thickness - 1) / 2;

    for (int row = 0; row < thickness; row++)
    {
      for (int col = 0; col < thickness; col++)
      {
        int targetX = x + offset + col;
        int targetY = y + offset + row;

        if (targetX < 0 || targetX >= board.Width ||
            targetY < 0 || targetY >= board.Height)
          continue;

        board.SetCell(targetX, targetY, cell);
      }
    }
  }

  // Проверка координат
  private static void CheckCoordinates(
      IBoard board,
      int x,
      int y)
  {
    if (x < 0 || x >= board.Width ||
        y < 0 || y >= board.Height)
    {
      throw new ArgumentOutOfRangeException(
          "Координаты находятся за пределами доски.");
    }
  }
}
