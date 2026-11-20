
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
      int width,
      int height)
  {
    ArgumentNullException.ThrowIfNull(board);

    if (width <= 0)
      throw new ArgumentOutOfRangeException(nameof(width));

    if (height <= 0)
      throw new ArgumentOutOfRangeException(nameof(height));

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
      DrawSizedPoint(board, x, y, cell, width, height);

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

  private static void DrawSizedPoint(
      IBoard board,
      int x,
      int y,
      BoardCell cell,
      int width,
      int height)
  {
    int offsetX = -(width - 1) / 2;
    int offsetY = -(height - 1) / 2;

    for (int row = 0; row < height; row++)
    {
      for (int col = 0; col < width; col++)
      {
        int targetX = x + offsetX + col;
        int targetY = y + offsetY + row;

        if (targetX < 0 || targetX >= board.Width ||
            targetY < 0 || targetY >= board.Height)
          continue;

        board.SetCell(targetX, targetY, cell);
      }
    }
  }

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
