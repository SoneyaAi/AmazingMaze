using Microsoft.Xna.Framework;

namespace AmazingMazeDesktop;

public static class Conversions
{
    public static Point WorldToCell(Vector2 world)
    {
        return (world / EngineSettings.CellSize).ToPoint();
    }

    public static Vector2 CellToWorld(Point cell)
    {
        return (cell.ToVector2() + new Vector2(0.5f)) * EngineSettings.CellSize;
    }
}