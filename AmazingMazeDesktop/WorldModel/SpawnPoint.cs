

using Microsoft.Xna.Framework;

namespace AmazingMazeDesktop.WorldModel;

public class SpawnPoint
{
    public Point TileCoordinates { get; set; }
    public bool IsPlayerSpawnFromLower { get; set; } = false;
    public bool IsPlayerSpawnFromHigher { get; set; } = false;
}