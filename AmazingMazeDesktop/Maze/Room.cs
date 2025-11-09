using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Labyrinthian;

public class Room
{
    public int Id;
    public List<Point> Cells = new();
    public Point[] EntryPath = new Point[2];
}