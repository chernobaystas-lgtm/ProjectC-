
namespace OwnProject.Board.Tools;

using System;
using OwnProject.Board;
using OwnProject.Board.InterFaceForB;
using OwnProject.Board.Methods;

public class TriangleTool : IDrawingTool
{
  public string Name => "Triangle";

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

    int left = Math.Min(startX, endX);
    int right = Math.Max(startX, endX);

    int top = Math.Min(startY, endY);
    int bottom = Math.Max(startY, endY);

    int topX = left + (right - left) / 2;
    int topY = top;

    int leftX = left;
    int leftY = bottom;

    int rightX = right;
    int rightY = bottom;

    LineRasterizer.DrawLine(
        board, topX, topY, leftX, leftY, cell, width, height);

    LineRasterizer.DrawLine(
        board, topX, topY, rightX, rightY, cell, width, height);

    LineRasterizer.DrawLine(
        board, leftX, leftY, rightX, rightY, cell, width, height);
  }
}
