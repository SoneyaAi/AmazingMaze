using System.Collections.Generic;

namespace AmazingMazeDesktop.WorldModel;

public class Dungeon
{
    public int Seed { get; set; }
    public List<Level> Levels { get; set; } = [];
}