using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Labyrinthian;

public class LabyrinthianMaze
{
    public OrthogonalMaze Map { get; private set; }
    public int[,] RoomsMap { get; private set; }

    public void Generate(int width, int height, int roomsCount)
    {
        var excluded = new List<GridPoint2D>();
        RoomsMap = CreateRoomsMap(width, height, roomsCount);
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
        Map = new OrthogonalMaze(width, height, excludedPredicate);
        var generator = new PrimGeneration(Map);
        generator.Generate();
    }

    // Rooms creation
    private int[,] CreateRoomsMap(int width, int height, int roomsCount)
    {
        var map = new int[height, width];

        var rng = new Random();
        for (var i = 0; i < roomsCount; i++)
        {
            PlaceSquare(map, rng, i + 2);
        }

        return map;
    }

    private void PlaceSquare(int[,] g, Random rng, int roomId)
    {
        var n = g.GetLength(0);
        var m = g.GetLength(1);

        var k = rng.Next(2) == 0 ? 2 : 3;

        while (true)
        {
            var r = rng.Next(0, n - k + 1);
            var c = rng.Next(0, m - k + 1);

            if (CanPlace(g, r, c, k))
            {
                Fill(g, r, c, k, roomId);
                return;
            }
        }
    }

    private bool CanPlace(int[,] g, int r, int c, int k)
    {
        for (var i = 0; i < k; i++)
        for (var j = 0; j < k; j++)
            if (g[r + i, c + j] != 0)
                return false;
        return true;
    }

    private void Fill(int[,] g, int r, int c, int k, int val)
    {
        for (var i = 0; i < k; i++)
        for (var j = 0; j < k; j++)
            g[r + i, c + j] = val;
    }

    // Pathfinding
    public List<GridPoint2D> GetPath(Point start, Point destination)
    {
        var path = new LabyrinthianPathfinder(Map, start, destination);
        return path.PathAsGridPoints;
    }
}