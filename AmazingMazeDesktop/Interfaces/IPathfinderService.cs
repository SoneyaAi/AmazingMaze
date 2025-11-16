using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AmazingMazeDesktop.Interfaces;

public interface IPathfinderService
{
    public Queue<Point> GetPath(Point startPoint, Point endPoint);
}