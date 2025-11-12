using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Labyrinthian;
using Microsoft.Xna.Framework;

namespace AmazingMazeDesktop;

public class WorldLevel(MazeStructure mazeStructure)
{
    public (int a, int b) GetFathestRooms()
    {
        // Every pair
        List<(int a, int b)> roomPairs = [];
        for (int i = 0; i < mazeStructure.Rooms.Count; i++)
        {
            for (int j = i + 1; j < mazeStructure.Rooms.Count; j++)
            {
                roomPairs.Add((mazeStructure.Rooms.Keys.ToArray()[i], mazeStructure.Rooms.Keys.ToArray()[j]));
            }
        }

        var ordered = roomPairs.OrderByDescending(tuple =>
            Vector2.Distance(mazeStructure.Rooms[tuple.a].EntryPath[0].ToVector2(),
                mazeStructure.Rooms[tuple.b].EntryPath[0].ToVector2())
        );
        return ordered.First();
    }
}