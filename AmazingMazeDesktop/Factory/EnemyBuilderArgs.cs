using Microsoft.Xna.Framework;

namespace AmazingMazeDesktop.Factory;

public class EnemyBuilderArgs : IBuilderArgs
{
    public required Vector2 Position;
    public required float Speed;
}