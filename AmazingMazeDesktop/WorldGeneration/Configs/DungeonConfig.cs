using System;

namespace AmazingMazeDesktop.WorldGeneration.Configs;

public class DungeonConfig
{
    public int Seed { get; set; } = Guid.NewGuid().GetHashCode();
}