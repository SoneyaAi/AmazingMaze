using AmazingMazeDesktop.Interfaces;
using AmazingMazeDesktop.Services;

namespace AmazingMazeDesktop;

public class GameServices()
{
    public IPathfinderService PathfinderService { get; set; } 
    public IPlayerTracker PlayerTracker { get; set; } 
    public ICoordsTranslationService CoordinatesTranslator { get; set; } 
}