using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Labyrinthian;

public class MazeStructureRoom
{
    public int Id;
    public List<Point> Cells = new();
    public Point EntryCell = new Point();
}