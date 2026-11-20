
namespace OwnProject.Board.Rendering;

using System;
using OwnProject.Board;
using OwnProject.Board.InterFaceForB;

public class BoardRenderer
{
    private readonly IBoard board;
    private bool firstRender = true;

    public BoardRenderer(IBoard board)
    {
        this.board = board
            ?? throw new ArgumentNullException(nameof(board));
    }

    public void Render(
        BoardCursor? cursor = null,
        int startX = 0,
        int startY = 0,
        MiniBar? miniBar = null)
    {
        if (startX < 0 || startY < 0)
            throw new ArgumentOutOfRangeException();

        if (firstRender)
        {
            Console.Clear();
            firstRender = false;
        }

        ConsoleColor oldForeground = Console.ForegroundColor;
        ConsoleColor oldBackground = Console.BackgroundColor;

        try
        {
            DrawBoard(cursor, startX, startY);

            if (miniBar != null)
                DrawMiniBar(miniBar, startX, startY);
        }
        finally
        {
            Console.ForegroundColor = oldForeground;
            Console.BackgroundColor = oldBackground;
        }
    }

    private void DrawBoard(
        BoardCursor? cursor,
        int startX,
        int startY)
    {
        ConsoleColor oldBackground = Console.BackgroundColor;

        WriteAt(
            startX,
            startY,
            "┌" + new string('─', board.Width) + "┐");

        for (int y = 0; y < board.Height; y++)
        {
            Console.SetCursorPosition(startX, startY + y + 1);

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write('│');

            for (int x = 0; x < board.Width; x++)
            {
                BoardCell cell = board.GetCell(x, y);

                bool isCursor =
                    cursor != null &&
                    cursor.X == x &&
                    cursor.Y == y;

                char symbol = cell.Symbol == '\0'
                    ? ' '
                    : cell.Symbol;

                Console.ForegroundColor = isCursor
                    ? ConsoleColor.White
                    : cell.Color;

                Console.BackgroundColor = isCursor
                    ? ConsoleColor.DarkGreen
                    : oldBackground;

                Console.Write(symbol);
            }

            Console.BackgroundColor = oldBackground;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write('│');
        }

        WriteAt(
            startX,
            startY + board.Height + 1,
            "└" + new string('─', board.Width) + "┘");
    }

    private void DrawMiniBar(
        MiniBar miniBar,
        int offsetX,
        int offsetY)
    {
        int x = miniBar.X + offsetX;
        int y = miniBar.Y + offsetY;

        // Мінімальна ширина та висота панелі
        int width = Math.Max(miniBar.Width, 23);
        int height = Math.Max(miniBar.Height, 16);

        string[] rows = new string[height];

        for (int i = 0; i < height; i++)
            rows[i] = string.Empty;

        SetRow(rows, 0, "ІНСТРУМЕНТИ");
        SetRow(rows, 1, new string('─', width));

        for (int i = 0; i < miniBar.MenuItems.Count; i++)
        {
            string prefix =
                i == miniBar.SelectedMenuIndex ? "> " : "  ";

            string translatedName = TranslateMenuItem(
                miniBar.MenuItems[i]);

            SetRow(
                rows,
                i + 2,
                prefix + (i + 1) + ". " + translatedName);
        }

        SetRow(rows, 9, new string('─', width));
        SetRow(rows, 10, "← Скасувати");
        SetRow(rows, 11, "→ Повторити");
        SetRow(rows, 12, new string('─', width));

        SetRow(rows, 14, "Tab: Полотно / меню");
        SetRow(rows, 15, "Esc: Вихід");

        WriteAt(
            x,
            y,
            "┌" + new string('─', width) + "┐");

        for (int i = 0; i < height; i++)
        {
            Console.SetCursorPosition(x, y + i + 1);

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write('│');

            int menuIndex = i - 2;

            bool active = IsActiveMenuItem(
                miniBar,
                menuIndex);

            bool selected =
                menuIndex == miniBar.SelectedMenuIndex;

            if (active)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (selected)
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.Write(FitRow(rows[i], width));

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write('│');
        }

        WriteAt(
            x,
            y + height + 1,
            "└" + new string('─', width) + "┘");
    }

    private string TranslateMenuItem(string item)
    {
        return item switch
        {
            "Рисовать" => "Малювати",
            "Стирать" => "Стирати",
            "Фигуры и линия" => "Фігури та лінія",
            "Цвет" => "Колір",
            "Символ" => "Символ",
            "Размер" => "Розмір",

            "Малювати" => "Малювати",
            "Стирати" => "Стирати",
            "Фігури та лінія" => "Фігури та лінія",
            "Колір" => "Колір",
            "Розмір" => "Розмір",

            _ => item
        };
    }

    private bool IsActiveMenuItem(
        MiniBar miniBar,
        int menuIndex)
    {
        string tool = miniBar.ActiveTool.Name;

        return menuIndex switch
        {
            0 => tool == "Brush",
            1 => tool == "Eraser",
            2 => tool == "Line"
                 || tool == "Rectangle"
                 || tool == "Triangle"
                 || tool == "Ellipse",
            _ => false
        };
    }

    private void SetRow(
        string[] rows,
        int index,
        string value)
    {
        if (index >= 0 && index < rows.Length)
            rows[index] = value;
    }

    private string FitRow(string value, int width)
    {
        if (value.Length > width)
            value = value.Substring(0, width);

        return value.PadRight(width);
    }

    private void WriteAt(int x, int y, string value)
    {
        Console.SetCursorPosition(x, y);
        Console.Write(value);
    }
}
