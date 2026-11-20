using System;
using OwnProject.Board.InterFaceForB;

namespace OwnProject.Board.Methods;

public interface IDrawingTool
{
    string Name { get; }

    void Execute(
        IBoard board,
        int startX,
        int startY,
        int endX,
        int endY,
        BoardCell cell,
        int width,
        int height);
}
