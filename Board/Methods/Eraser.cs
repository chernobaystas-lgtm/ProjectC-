
namespace OwnProject.Board.Tools;

using System;
using OwnProject.Board;
using OwnProject.Board.InterFaceForB;
using OwnProject.Board.Methods;

public class EraserTool : IDrawingTool
{
  public string Name => "Eraser";

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
    BoardCell emptyCell = new BoardCell
    {
      Symbol = ' ',
      Color = ConsoleColor.White
    };

    LineRasterizer.DrawLine(
        board,
        startX,
        startY,
        endX,
        endY,
        emptyCell,
         width,
 height);
  }
}

