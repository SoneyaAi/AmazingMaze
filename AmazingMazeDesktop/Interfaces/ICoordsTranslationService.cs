using AmazingMazeDesktop.WorldModel;
using Microsoft.Xna.Framework;

namespace AmazingMazeDesktop.Interfaces;

public interface ICoordsTranslationService
{
    public Point WorldToCell(Vector2 world);

    public Vector2 CellToWorld(Point cell);

    public Room CellToRoom(Point cell);
}