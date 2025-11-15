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

        // Tilemap
        level.TileMap = new TileMap();
        var map = level.MazeStructure.Map;
        int height = map.GetLength(0); // Y
        int width = map.GetLength(1); // X

        level.TileMap.Tiles = new Tile[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int cell = map[y, x];

                level.TileMap.Tiles[y, x] = new Tile
                {
                    Type = cell == 1 ? TileType.Wall : TileType.Floor,
                    IsPassable = cell != 1
                };
            }
        }

        // Entry/Exit room
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

        // SpawnPoints
        foreach (var room in level.MazeStructure.Rooms)
        {
            var spawnTile = room.Value.Cells.Shuffle(random).First();
            var spawnPoint = new SpawnPoint
            {
                IsPlayerSpawn = room.Key == level.EntryRoomId,
                TileCoordinates = spawnTile
            };
            level.SpawnPoints.Add(spawnPoint);
            level.TileMap.Tiles[spawnTile.Y, spawnTile.X].IsOccupied = true;
        }

        // Triggers
        var entryTile = level.MazeStructure.Rooms[level.EntryRoomId].Cells
            .Where(p => !level.TileMap.Tiles[p.Y, p.X].IsOccupied)
            .ToList()
            .Shuffle(random)
            .First();

        level.Triggers.Add(new Trigger
        {
            TileCoordinates = entryTile,
            TriggerType = TriggerType.OnEnter,
            Action = TriggerAction.LoadPreviousLevel
        });

        level.TileMap.Tiles[entryTile.Y, entryTile.X].IsOccupied = true;


        var exitTile = level.MazeStructure.Rooms[level.ExitRoomId].Cells
            .Where(p => !level.TileMap.Tiles[p.Y, p.X].IsOccupied)
            .ToList()
            .Shuffle(random)
            .First();

        level.Triggers.Add(new Trigger
        {
            TileCoordinates = exitTile,
            TriggerType = TriggerType.OnEnter,
            Action = TriggerAction.LoadNextLevel
        });

        level.TileMap.Tiles[exitTile.Y, exitTile.X].IsOccupied = true;


        return level;
    }
}