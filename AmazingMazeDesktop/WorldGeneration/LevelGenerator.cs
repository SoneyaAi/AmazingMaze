using System;
using AmazingMazeDesktop.WorldGeneration.Configs;
using AmazingMazeDesktop.WorldModel;

namespace AmazingMazeDesktop.WorldGeneration;

public class LevelGenerator()
{
    public Level Generate(ConfigsPackage configs)
    {
        var config = configs.LevelConfig;
        var random = new Random(config.Seed);
        var level = new Level();
        var roomsCount = config.Width * config.Height / 200;
        level.MazeStructure = new MazeStructure(config.Width, config.Height, roomsCount);
        
        
        
        
        
        return level;
    }
}