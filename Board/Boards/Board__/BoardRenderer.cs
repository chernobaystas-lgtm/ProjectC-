
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

    // Полное отображение рабочей области
    public void Render(
        BoardCursor? cursor = null,
        int startX = 0,
        int startY = 0,
        MiniBar? miniBar = null)
    {

        // Отображение доски в терминале

        /* Renderer:
1. Получает размер доски.
2. Проходит по каждой клетке.
3. Получает символ и цвет через GetCell().
4. Устанавливает цвет текста в консоли.
5. Выводит символ в нужную позицию.

*/
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

    // Отрисовка холста
    private void DrawBoard(
        BoardCursor? cursor,
        int startX,
        int startY)
    {
        ConsoleColor oldBackground = Console.BackgroundColor;

        // Верхняя граница
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

        // Нижняя граница
        WriteAt(
            startX,
            startY + board.Height + 1,
            "└" + new string('─', board.Width) + "┘");
    }

    // Отрисовка панели инструментов
    private void DrawMiniBar(
        MiniBar miniBar,
        int offsetX,
        int offsetY)
    {
        int x = miniBar.X + offsetX;
        int y = miniBar.Y + offsetY;

        int width = miniBar.Width;
        int height = miniBar.Height;

        if (width <= 0 || height <= 0)
            return;

        string[] rows = new string[height];

        for (int i = 0; i < height; i++)
            rows[i] = string.Empty;

        // Заголовок
        SetRow(rows, 0, "ИНСТРУМЕНТЫ");
        SetRow(rows, 1, new string('─', width));

        // Основные разделы
        for (int i = 0; i < miniBar.MenuItems.Count; i++)
        {
            string prefix =
                i == miniBar.SelectedMenuIndex ? "> " : "  ";

            SetRow(
                rows,
                i + 2,
                prefix + (i + 1) + ". " + miniBar.MenuItems[i]);
        }

        // Нижние действия
        SetRow(rows, 9, new string('─', width));
        SetRow(rows, 10, "← Undo");
        SetRow(rows, 11, "→ Redo");
        SetRow(rows, 12, new string('─', width));
        SetRow(rows, 13, "↓ Скачать PNG");
        SetRow(rows, 14, "Tab: Холст / панель");
        SetRow(rows, 15, "Esc: Выход");

        // Верхняя граница панели
        Console.ForegroundColor = ConsoleColor.Gray;

        WriteAt(
            x,
            y,
            "┌" + new string('─', width) + "┐");

        // Содержимое панели
        for (int i = 0; i < height; i++)
        {
            Console.SetCursorPosition(x, y + i + 1);

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;

            Console.Write('│');

            bool selected =
                i == miniBar.SelectedMenuIndex + 2;

            if (selected)
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
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

        // Нижняя граница панели
        WriteAt(
            x,
            y + height + 1,
            "└" + new string('─', width) + "┘");
    }

    // Добавление строки в панель
    private void SetRow(
        string[] rows,
        int index,
        string value)
    {
        if (index >= 0 && index < rows.Length)
            rows[index] = value;
    }

    // Подгонка текста под ширину панели
    private string FitRow(string value, int width)
    {
        if (value.Length > width)
            value = value.Substring(0, width);

        return value.PadRight(width);
    }

    // Вывод в определённую позицию
    private void WriteAt(int x, int y, string value)
    {
        Console.SetCursorPosition(x, y);
        Console.Write(value);
    }
}
