using System;
using System.Globalization;
using System.Text;

namespace OwnProject.Board;

public class Cursor
{
  public char Symbol { get; private set; }
  public ConsoleColor Color { get; private set; }
  public int Thickness { get; private set; }

  public Cursor()
  {
  }

  public void ChangeSymbol(char symbol)
  {
    bool isAssigned = char.GetUnicodeCategory(symbol) != UnicodeCategory.OtherNotAssigned;
    if (isAssigned == true) Symbol = symbol;
    else { throw new Exception(); }

  }

  public void ChangeColor(ConsoleColor color)
  {
    // Изменение цвета
  }

  public void ChangeThickness(int thickness)
  {
    // Изменение толщины
  }
}
