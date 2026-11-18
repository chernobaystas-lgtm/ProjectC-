using System;
using OwnProject.Board;

namespace OwnProject.Board.InterFaceForB;

public interface IBoard
{
  int Width { get; }
  int Height { get; }

  void SetCell(int x, int y, BoardCell cell);
  BoardCell GetCell(int x, int y);
}
