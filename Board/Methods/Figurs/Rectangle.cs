
namespace OwnProject.Board.Tools;

using System;
using OwnProject.Board;
using OwnProject.Board.InterFaceForB;
using OwnProject.Board.Methods;

public class RectangleTool : IDrawingTool
{
  public string Name => "Rectangle";

  public void Execute(
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

    // Определяем границы прямоугольника
    int left = Math.Min(startX, endX);
    int right = Math.Max(startX, endX);

    int top = Math.Min(startY, endY);
    int bottom = Math.Max(startY, endY);

    // Верхняя сторона
    LineRasterizer.DrawLine(
        board, left, top, right, top, cell, thickness);

    // Нижняя сторона
    LineRasterizer.DrawLine(
        board, left, bottom, right, bottom, cell, thickness);

    // Левая сторона
    LineRasterizer.DrawLine(
        board, left, top, left, bottom, cell, thickness);

    // Правая сторона
    LineRasterizer.DrawLine(
        board, right, top, right, bottom, cell, thickness);
  }
}

