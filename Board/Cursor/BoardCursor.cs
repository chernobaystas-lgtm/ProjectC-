using System;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using OwnProject.Board.InterFaceForB;

namespace OwnProject.Board;

public class BoardCursor
{
  private readonly IBoard board;
  /* readonly означает, что после присвоения значения в конструкторе мы не сможем заменить ссылку на другой объект.*/

  public int X { get; private set; }
  public int Y { get; private set; }

  public BoardCursor(IBoard board)
  {
    this.board = board
        ?? throw new ArgumentNullException(nameof(board));

    X = 0;
    Y = 0;
  }

  // Перемещение влево
  public void MoveLeft()
  {
    if (X > 0)
      X--;
  }

  // Перемещение вправо
  public void MoveRight()
  {
    if (X < board.Width - 1)
      X++;
  }

  // Перемещение вверх
  public void MoveUp()
  {
    if (Y > 0)
      Y--;
  }

  // Перемещение вниз
  public void MoveDown()
  {
    if (Y < board.Height - 1)
      Y++;
  }
}
