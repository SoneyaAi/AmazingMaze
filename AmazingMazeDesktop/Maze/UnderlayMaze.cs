using System;
using System.Collections.Generic;
using Labyrinthian;

namespace AmazingMazeDesktop.Maze.Underlay;

/// <summary>
/// Adapter for Labyrinthian library to create base schema of maze with addition of empty spaces for rooms
/// </summary>
public class UnderlayMaze
{
    public OrthogonalMaze Maze { get; private set; }
    public UnderlayRoomsMap RoomsMap { get; private set; }

    public void Generate(int width, int height, int roomsCount)
    {
        RoomsMap = new UnderlayRoomsMap(width, height);
        RoomsMap.Generate(roomsCount);
        
        var excluded = new List<GridPoint2D>();
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                if (RoomsMap[y, x] >= 2)
                {
                    excluded.Add(new GridPoint2D(y, x));
                }
            }
        }

        Predicate<GridPoint2D> excludedPredicate = point2D => !excluded.Contains(point2D);
        Maze = new OrthogonalMaze(width, height, excludedPredicate);
        var generator = new PrimGeneration(Maze);
        generator.Generate();
    }
}