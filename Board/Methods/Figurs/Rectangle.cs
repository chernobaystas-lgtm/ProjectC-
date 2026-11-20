
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

        LineRasterizer.DrawLine(
            board, left, top, right, top, cell, width, height);

        LineRasterizer.DrawLine(
            board, left, bottom, right, bottom, cell, width, height);

        LineRasterizer.DrawLine(
            board, left, top, left, bottom, cell, width, height);

        LineRasterizer.DrawLine(
            board, right, top, right, bottom, cell, width, height);
    }
}
