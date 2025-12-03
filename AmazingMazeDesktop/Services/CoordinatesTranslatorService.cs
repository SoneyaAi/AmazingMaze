using System.Linq;
using AmazingMazeDesktop.Interfaces;
using AmazingMazeDesktop.WorldModel;
using Microsoft.Xna.Framework;

namespace AmazingMazeDesktop;

public class CoordinatesTranslatorService(GameContext context) : ICoordsTranslationService
{
    public Point WorldToCell(Vector2 world)
    {
        return (world / EngineSettings.CellSize).ToPoint();
    }

    public Vector2 CellToWorld(Point cell)
    {
        return (cell.ToVector2() + new Vector2(0.5f)) * EngineSettings.CellSize;
    }
    
    public Room CellToRoom(Point cell)
    {
        var room = context.CurrentLevel.Rooms.FirstOrDefault(x => x.Tiles.Select(y => y.Position).Contains(cell));
        return room;
    }
}