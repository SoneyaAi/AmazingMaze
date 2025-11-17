using AmazingMazeDesktop.Services;

namespace AmazingMazeDesktop;

public class GameServices(GameContext context)
{
    public AstarPathfinderService PathfinderService { get; private set; } = new AstarPathfinderService(context);
    public PlayerTracker PlayerTracker { get; private set; } = new PlayerTracker(context);
}