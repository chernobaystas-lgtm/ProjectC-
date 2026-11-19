
namespace OwnProject.Board.History;

using System;
using System.Collections.Generic;
using OwnProject.Board.InterFaceForB;

public class BoardHistory : IBoardHistory
{
  private readonly IBoard board;

  private readonly List<BoardCell[,]> undoHistory = new();
  private readonly List<BoardCell[,]> redoHistory = new();

  private const int MaxHistory = 50;

  public bool CanUndo => undoHistory.Count > 0;
  public bool CanRedo => redoHistory.Count > 0;

  public BoardHistory(IBoard board)
  {
    this.board = board
        ?? throw new ArgumentNullException(nameof(board));
  }

  // Создание снимка доски
  private BoardCell[,] CreateSnapshot()
  {
    /*мы сохраняем именно отдельную копию, а не ссылку на исходный массив.*/
    BoardCell[,] snapshot =
        new BoardCell[board.Height, board.Width];

    for (int y = 0; y < board.Height; y++)
    {
      for (int x = 0; x < board.Width; x++)
      {
        snapshot[y, x] = board.GetCell(x, y);
      }
    }

    return snapshot;
  }

  // Восстановление снимка
  private void RestoreSnapshot(BoardCell[,] snapshot)
  {
    for (int y = 0; y < board.Height; y++)
    {
      for (int x = 0; x < board.Width; x++)
      {
        board.SetCell(x, y, snapshot[y, x]);
      }
    }
  }

  // Сохранение текущего состояния перед изменением
  public void SaveState()
  {
    undoHistory.Add(CreateSnapshot());

    // Ограничиваем размер истории
    if (undoHistory.Count > MaxHistory)
      undoHistory.RemoveAt(0);

    // Новое действие удаляет историю повтора
    redoHistory.Clear();
  }

  // Отмена последнего действия
  public bool Undo()
  {
    if (!CanUndo)
      return false;

    redoHistory.Add(CreateSnapshot());

    BoardCell[,] previousState =
        undoHistory[undoHistory.Count - 1];

    undoHistory.RemoveAt(undoHistory.Count - 1);

    RestoreSnapshot(previousState);

    return true;
  }

  // Повтор отменённого действия
  public bool Redo()
  {
    if (!CanRedo)
      return false;

    undoHistory.Add(CreateSnapshot());

    BoardCell[,] nextState =
        redoHistory[redoHistory.Count - 1];

    redoHistory.RemoveAt(redoHistory.Count - 1);

    RestoreSnapshot(nextState);

    return true;
  }


  // Полная очистка истории
  public void ClearHistory()
  {
    undoHistory.Clear();
    redoHistory.Clear();
  }
}
