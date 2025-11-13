using System;

namespace AmazingMazeDesktop.WorldGeneration.Configs;

public class LevelConfig
{
    public int Seed { get; set; } = Guid.NewGuid().GetHashCode();
    public int Width { get; set; } = 20;
    public int Height { get; set; } = 30;
}