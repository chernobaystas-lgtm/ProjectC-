
namespace OwnProject.Board.Tools;

using System;
using OwnProject.Board;
using OwnProject.Board.InterFaceForB;
using OwnProject.Board.Methods;

public class EllipseTool : IDrawingTool
{
  public string Name => "Ellipse";

  public void Execute(
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

    int left = Math.Min(startX, endX);
    int right = Math.Max(startX, endX);
    int top = Math.Min(startY, endY);
    int bottom = Math.Max(startY, endY);

    double centerX = (left + right) / 2.0;
    double centerY = (top + bottom) / 2.0;

    double radiusX = (right - left) / 2.0;
    double radiusY = (bottom - top) / 2.0;

    if (radiusX == 0 || radiusY == 0)
    {
      LineRasterizer.DrawLine(
          board, left, top, right, bottom, cell, width, height);

      return;
    }

    int segments = Math.Max(
        24,
        (int)Math.Ceiling(
            8 * Math.PI * Math.Max(radiusX, radiusY)));

    int previousX = 0;
    int previousY = 0;

    for (int i = 0; i <= segments; i++)
    {
      double angle = 2 * Math.PI * i / segments;

      int x = (int)Math.Round(
          centerX + radiusX * Math.Cos(angle),
          MidpointRounding.AwayFromZero);

      int y = (int)Math.Round(
          centerY + radiusY * Math.Sin(angle),
          MidpointRounding.AwayFromZero);

      x = Math.Clamp(x, left, right);
      y = Math.Clamp(y, top, bottom);

      if (i > 0)
      {
        LineRasterizer.DrawLine(
            board,
            previousX,
            previousY,
            x,
            y,
            cell,
            width,
            height);
      }

      previousX = x;
      previousY = y;
    }
  }

  private void CheckCoordinates(
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
