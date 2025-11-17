using AmazingMazeDesktop.Interfaces;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace AmazingMazeDesktop.Services;

public class PlayerTracker(GameContext context) : IPlayerTracker
{
    public Vector2 GetPlayerPositionWorld()
    {
        var entity = context.World.World.GetEntity(context.PlayerEntityId);
        var position = entity.Get<Transform2>().Position;
        return position;
    }

    public Point GetPlayerPositionCell()
    {
        return Conversions.WorldToCell(GetPlayerPositionWorld());
    }
}