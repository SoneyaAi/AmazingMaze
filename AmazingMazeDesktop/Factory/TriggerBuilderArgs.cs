using Microsoft.Xna.Framework;

namespace AmazingMazeDesktop.Factory;

public class TriggerBuilderArgs : IBuilderArgs
{
    public required Vector2 Position;
    public required TriggerAction  Action;
    public required TriggerType Type;
}