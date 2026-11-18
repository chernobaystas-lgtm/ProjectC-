namespace OwnProject.Board.Rendering;

using System;
using OwnProject.Board;
using OwnProject.Board.InterFaceForB;

public class BoardRenderer
{
    private readonly IBoard board;

    public BoardRenderer(IBoard board)
    {
        this.board = board
            ?? throw new ArgumentNullException(nameof(board));
    }

    // Отображение доски в терминале
    public void Render(int startX = 0, int startY = 0)
    {

        /* Renderer:
    1. Получает размер доски.
    2. Проходит по каждой клетке.
    3. Получает символ и цвет через GetCell().
    4. Устанавливает цвет текста в консоли.
    5. Выводит символ в нужную позицию.

    */
        if (startX < 0 || startY < 0)
            throw new ArgumentOutOfRangeException();

        if (startX + board.Width > Console.BufferWidth ||
            startY + board.Height > Console.BufferHeight)
        {
            throw new InvalidOperationException(
                "Доска не помещается в окно терминала.");
        }

        ConsoleColor originalColor = Console.ForegroundColor;

        try
        {
            for (int y = 0; y < board.Height; y++)
            {
                Console.SetCursorPosition(startX, startY + y);

                for (int x = 0; x < board.Width; x++)
                {
                    BoardCell cell = board.GetCell(x, y);

                    Console.ForegroundColor = cell.Color;

                    char symbol = cell.Symbol == '\0'
                        ? ' '
                        : cell.Symbol;

                    Console.Write(symbol);
                }
            }
        }
        finally
        {
            Console.ForegroundColor = originalColor;
        }
    }


}
