
using System;
using System.Globalization;

namespace OwnProject.Board;

public class Cursor
{
    public char Symbol { get; private set; }
    public ConsoleColor Color { get; private set; }

    public int Width { get; private set; }
    public int Height { get; private set; }

    public Cursor()
    {
        Symbol = '#';
        Color = ConsoleColor.White;
        Width = 1;
        Height = 1;
    }

    public void ChangeSymbol(char symbol)
    {
        UnicodeCategory category = char.GetUnicodeCategory(symbol);

        if (char.IsControl(symbol) ||
            char.IsWhiteSpace(symbol) ||
            category == UnicodeCategory.OtherNotAssigned ||
            category == UnicodeCategory.Surrogate)
        {
            throw new ArgumentException(
                "Недопустимый символ для рисования.",
                nameof(symbol));
        }

        Symbol = symbol;
    }

    public void ChangeColor(ConsoleColor color)
    {
        if (!Enum.IsDefined(typeof(ConsoleColor), color))
            throw new ArgumentOutOfRangeException(nameof(color));

        Color = color;
    }

    public void ChangeSize(int width, int height)
    {
        if (width < 1 || width > 20)
            throw new ArgumentOutOfRangeException(nameof(width));

        if (height < 1 || height > 20)
            throw new ArgumentOutOfRangeException(nameof(height));

        Width = width;
        Height = height;
    }
}
