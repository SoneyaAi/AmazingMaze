using System;
using System.Collections.Generic;
using System.Linq;
using AmazingMazeDesktop.WorldGeneration.Configs;
using AmazingMazeDesktop.WorldModel;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Collections;

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

        // Get two farthest rooms
        List<(int a, int b)> roomPairs = [];
        for (int i = 0; i < level.MazeStructure.Rooms.Count; i++)
        {
            for (int j = i + 1; j < level.MazeStructure.Rooms.Count; j++)
            {
                roomPairs.Add(
                    (level.MazeStructure.Rooms.Keys.ToArray()[i], level.MazeStructure.Rooms.Keys.ToArray()[j]));
            }
        }

        var ordered = roomPairs.OrderByDescending(tuple =>
            Vector2.Distance(level.MazeStructure.Rooms[tuple.a].EntryPath[0].ToVector2(),
                level.MazeStructure.Rooms[tuple.b].EntryPath[0].ToVector2())
        );
        var pair = ordered.First();
        level.EntryRoomId = pair.a;
        level.ExitRoomId = pair.b;

        // Create SpawnPoints
        foreach (var room in level.MazeStructure.Rooms)
        {
            var spawnPoint = new SpawnPoint
            {
                IsPlayerSpawn = room.Key == level.EntryRoomId,
                TileCoordinates = room.Value.Cells.Shuffle(random).First()
            };
            level.SpawnPoints.Add(spawnPoint);
        }


        return level;
    }
}