namespace OwnProject.Board.History;

public interface IBoardHistory
{
  bool CanUndo { get; }
  bool CanRedo { get; }

  void SaveState();

  bool Undo();
  bool Redo();

  void ClearHistory();
}
