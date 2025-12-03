using System.Linq;
using AmazingMazeDesktop.Interfaces;
using AmazingMazeDesktop.WorldModel;
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
        return context.Services.CoordinatesTranslator.WorldToCell(GetPlayerPositionWorld());
    }
    public Room GetPlayerPositionRoom()
    {
        var cell = GetPlayerPositionCell();
        var room = context.CurrentLevel.Rooms.FirstOrDefault(x => x.Tiles.Select(y => y.Position).Contains(cell));
        return room;
    }
}