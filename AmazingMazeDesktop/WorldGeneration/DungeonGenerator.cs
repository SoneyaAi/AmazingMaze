using System;
using AmazingMazeDesktop.WorldGeneration.Configs;
using AmazingMazeDesktop.WorldModel;

namespace AmazingMazeDesktop.WorldGeneration;

public class DungeonGenerator
{
    public Dungeon Generate(ConfigsPackage configs)
    {
        var config = configs.DungeonConfig;
        var rng = new Random(config.Seed);
        var levelGenerator = new LevelGenerator();
        var dungeon = new Dungeon();
        dungeon.Levels.Add(levelGenerator.Generate(configs));

        return dungeon;
    }
}