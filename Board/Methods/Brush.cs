
namespace OwnProject.Board.Tools;

using OwnProject.Board;
using OwnProject.Board.InterFaceForB;
using OwnProject.Board.Methods;

public class BrushTool : IDrawingTool
{
    public string Name => "Brush";

    public void Execute(
        IBoard board,
        int startX,
        int startY,
        int endX,
        int endY,
        BoardCell cell,
         int width,
        int height)
    {
        LineRasterizer.DrawLine(
            board,
            startX,
            startY,
            endX,
            endY,
            cell,
            width,
            height);

    }
}
