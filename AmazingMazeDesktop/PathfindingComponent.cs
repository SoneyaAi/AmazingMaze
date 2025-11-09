using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AmazingMazeDesktop;

public class PathfindingComponent
{
    public List<Point> Path;
    public bool PathRecalculateRequested;
}