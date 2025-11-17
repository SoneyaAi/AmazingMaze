using Microsoft.Xna.Framework;

namespace AmazingMazeDesktop.Interfaces;

public interface IPlayerTracker
{
    public Vector2 GetPlayerPositionWorld();
    public Point GetPlayerPositionCell();
}