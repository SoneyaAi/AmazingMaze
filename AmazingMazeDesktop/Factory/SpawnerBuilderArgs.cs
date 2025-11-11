using Microsoft.Xna.Framework;

namespace AmazingMazeDesktop.Factory;

public class SpawnerBuilderArgs : IBuilderArgs
{
    public required Vector2 Position;
    public required int TimeToSpawn;
}