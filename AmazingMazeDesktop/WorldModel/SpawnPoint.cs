

using Microsoft.Xna.Framework;

namespace AmazingMazeDesktop.WorldModel;

public class SpawnPoint
{
    public Point TileCoordinates { get; set; }
    public bool IsPlayerSpawn { get; set; } = false;
}