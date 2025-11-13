using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AmazingMazeDesktop.Components;

public class PathComponent
{
    public Queue<Point> Path = [];
    public bool RecalculateRequested ;
}