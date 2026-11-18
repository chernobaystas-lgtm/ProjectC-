using System;

namespace OwnProject.Board;

public struct BoardCell
{
    public char Symbol { get; set; }

    public ConsoleColor Color { get; set; }

    /* ConsoleColor — тип данных. Это встроенное перечисление (enum) из пространства имен System, которое содержит список доступных цветов консоли от 0 до 15*/
}