using System;
using System.Collections.Generic;

namespace AmazingMazeDesktop.WorldModel;

public class Level
{
    public Guid Id { get; } = Guid.NewGuid();
    public MazeStructure MazeStructure { get; set; }
    public TileMap TileMap { get; set; }
    public List<Room> Rooms { get; set; } = [];
    public List<SpawnPoint> SpawnPoints { get; set; } = [];
    public int EntryRoomId { get; set; }
    public int ExitRoomId { get; set; }
}