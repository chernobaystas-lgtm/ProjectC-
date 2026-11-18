using System;
using OwnProject.Board;
using OwnProject.Board.InterFaceForB;

namespace OwnProject.Board.Services;

public class BoardService
{
  private readonly IBoard board;

  public BoardService(IBoard board)
  {
    this.board = board
        ?? throw new ArgumentNullException(nameof(board));
  }

  // Очистка всей доски
  public void Clear()
  {
    for (int y = 0; y < board.Height; y++)
    {
      for (int x = 0; x < board.Width; x++)
      {
        board.SetCell(x, y, new BoardCell
        {
          Symbol = ' ',
          Color = ConsoleColor.White
        });
      }
    }
  }
}
