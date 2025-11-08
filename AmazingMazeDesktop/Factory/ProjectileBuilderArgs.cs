using Microsoft.Xna.Framework;

namespace AmazingMazeDesktop.Factory;

public class ProjectileBuilderArgs : IBuilderArgs
{
    public required Vector2 Position;
    public required float Velocity;
    public required Vector2 Direction;
    public required Vector2 Scale;
}