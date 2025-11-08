using Labyrinthian;
using Microsoft.Xna.Framework;

namespace AmazingMazeDesktop;

public static class Extensions
{
    public static Point ToPoint(this GridPoint2D gridPoint2D)
    {
        return new Point(gridPoint2D.Column, gridPoint2D.Row);
    }
}