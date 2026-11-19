
using System;
using System.Globalization;

namespace OwnProject.Board;

public class Cursor
{
    public char Symbol { get; private set; }
    public ConsoleColor Color { get; private set; }
    public int Thickness { get; private set; }

    // Начальные настройки инструмента
    public Cursor()
    {
        Symbol = '#';
        Color = ConsoleColor.White;
        Thickness = 1;
    }

    // Изменение символа
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

    // Изменение цвета
    public void ChangeColor(ConsoleColor color)
    {
        if (!Enum.IsDefined(typeof(ConsoleColor), color))
        {
            throw new ArgumentOutOfRangeException(
                nameof(color),
                "Такого цвета консоли не существует.");
        }

        Color = color;
    }

    // Изменение толщины
    public void ChangeThickness(int thickness)
    {
        if (thickness < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(thickness),
                "Толщина кисти должна быть не меньше 1.");
        }

        Thickness = thickness;
    }
}

