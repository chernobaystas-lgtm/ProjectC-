
namespace OwnProject.Board.Tools;

using OwnProject.Board;
using OwnProject.Board.InterFaceForB;
using OwnProject.Board.Methods;

public class LineTool : IDrawingTool
{
    public string Name => "Line";

    public void Execute(
        IBoard board,
        int startX,
        int startY,
        int endX,
        int endY,
        BoardCell cell,
        int thickness)
    {
        LineRasterizer.DrawLine(
            board,
            startX,
            startY,
            endX,
            endY,
            cell,
            thickness);
    }
}
