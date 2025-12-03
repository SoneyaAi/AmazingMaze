using Microsoft.Xna.Framework;

namespace AmazingMazeDesktop.WorldModel;

public class Tile
{
    public Point Position { get; set; }
    public TileType Type;
    public bool IsOccupied;
    public bool IsPassable;
    public bool IsSafeZone;
}