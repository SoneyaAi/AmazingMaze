using Microsoft.Xna.Framework;

namespace AmazingMazeDesktop.WorldModel;

public class Trigger
{
    public Point TileCoordinates { get; set; }
    public bool IsPlayerSpawn { get; set; } = false;
}