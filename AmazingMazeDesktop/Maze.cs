using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Labyrinthian;
using Labyrinthian.Svg;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using ToolsUtilities;

namespace AmazingMazeDesktop;

public class Maze
{
    public int[,] MazeSchema;
    private OrthogonalMaze _labyrinthianMaze;

    public Maze(int width, int height, int roomsCount = 2)
    {
        var oddWidth = width % 2 == 0 ? width + 1 : width;
        var oddHeight = height % 2 == 0 ? height + 1 : height;
        Generate(oddWidth, oddHeight,  roomsCount);
    }

    private void Generate(int width, int height, int roomsCount = 2)
    {
        // Generate Labyrinthian maze 
        var baseWidth = width / 2; 
        var baseHeight = height / 2;
        var roomsMap = GenerateLabyrinthianMaze(baseWidth, baseHeight, roomsCount);

        // Generate converted maze schema
        var convertedMaze = new int[height, width];
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                convertedMaze[y, x] = 1;
            }
        }

        for (var x = 0; x < _labyrinthianMaze.Columns; x++)
        {
            for (var y = 0; y < _labyrinthianMaze.Rows; y++)
            {
                var cell = _labyrinthianMaze[y, x];

                var translatedY = y * 2 + 1;
                var translatedX = x * 2 + 1;
                if (cell is null)
                {
                    if (roomsMap[y, x] >= 2)
                    {
                        var id = roomsMap[y, x];
                        if (x + 1 == _labyrinthianMaze.Columns || y + 1 == _labyrinthianMaze.Rows)
                            continue;
                        convertedMaze[translatedY, translatedX] = id;
                        if (roomsMap[y, x + 1] == id)
                            convertedMaze[translatedY, translatedX + 1] = id;
                        else
                            convertedMaze[translatedY, translatedX + 1] = 1;

                        if (roomsMap[y + 1, x] == id)
                            convertedMaze[translatedY + 1, translatedX] = id;
                        else if (roomsMap[y + 1, x] == 1)
                            convertedMaze[translatedY + 1, translatedX] = id;
                        else
                            convertedMaze[translatedY + 1, translatedX] = 1;

                        if (roomsMap[y, x + 1] == id && roomsMap[y + 1, x] == id)
                            convertedMaze[translatedY + 1, translatedX + 1] = id;
                    }

                    continue;
                }

                convertedMaze[translatedY, translatedX] = 0;
                convertedMaze[translatedY, translatedX + 1] =
                    _labyrinthianMaze.AreCellsConnected(cell, cell.DirectedNeighbors[0]) ? 0 : 1;
                convertedMaze[translatedY + 1, translatedX] =
                    _labyrinthianMaze.AreCellsConnected(cell, cell.DirectedNeighbors[2]) ? 0 : 1;
            }
        }

        MazeSchema = convertedMaze;
    }

    private int[,] GenerateLabyrinthianMaze(int width, int height, int roomsCount)
    {
        var excluded = new List<GridPoint2D>();
        var rooms = GetRoomsMap(width, height, roomsCount);
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                if (rooms[y, x] >= 2)
                {
                    excluded.Add(new GridPoint2D(y, x));
                }
            }
        }

        Predicate<GridPoint2D> excludedPredicate = point2D => !excluded.Contains(point2D);
        _labyrinthianMaze = new OrthogonalMaze(width, height, excludedPredicate);
        var generator = new PrimGeneration(_labyrinthianMaze);
        generator.Generate();
        return rooms;
    }

    public Queue<Point> GetPath(Point start, Point destination)
    {
        var startCell = TranslateGlobalToLabyrinthian(start);
        var destCell = TranslateGlobalToLabyrinthian(destination);

        var path = new LabyrinthianPathfinder(_labyrinthianMaze, startCell, destCell);
        Queue<Point> waypoints = [];

        var pathList = path.PathAsGridPoints;
        foreach (var gridPoint in pathList)
        {
            var waypoint = new Point(gridPoint.Column * 2 + 1, gridPoint.Row * 2 + 1);

            if (waypoints.Count > 0)
            {
                var last = waypoints.Last();
                var midpoint = new Point(
                    (last.X + waypoint.X) / 2,
                    (last.Y + waypoint.Y) / 2
                );

                waypoints.Enqueue(midpoint);
            }

            waypoints.Enqueue(waypoint);
        }

        if (!waypoints.Last().Equals(destination))
            waypoints.Enqueue(destination);
        while (waypoints.Contains(start))
            waypoints.Dequeue();

        return waypoints;
    }

    private Point TranslateGlobalToLabyrinthian(Point global)
    {
        var x = global.X / 2;
        var y = global.Y / 2;
        return new Point(x, y);
    }

    // Rooms creation
    private int[,] GetRoomsMap(int width, int height, int roomsCount)
    {
        var map = new int[height, width];

        var rng = new Random();
        for (var i = 0; i < roomsCount; i++)
        {
            PlaceSquare(map, rng, i + 2);
        }

        return map;
    }

    void PlaceSquare(int[,] g, Random rng, int roomId)
    {
        var n = g.GetLength(0);
        var m = g.GetLength(1);

        // losuj rozmiar 2x2 lub 3x3
        var k = 3; //rng.Next(2) == 0 ? 2 : 3;

        // próbuj aż się uda bez nachodzenia
        while (true)
        {
            var r = rng.Next(0, n - k + 1); // wiersz lewego-górnego rogu
            var c = rng.Next(0, m - k + 1); // kolumna lewego-górnego rogu

            if (CanPlace(g, r, c, k))
            {
                Fill(g, r, c, k, roomId);
                return;
            }
        }
    }

    bool CanPlace(int[,] g, int r, int c, int k)
    {
        for (var i = 0; i < k; i++)
        for (var j = 0; j < k; j++)
            if (g[r + i, c + j] != 0)
                return false; // unikaj nachodzenia
        return true;
    }

    void Fill(int[,] g, int r, int c, int k, int val)
    {
        for (var i = 0; i < k; i++)
        for (var j = 0; j < k; j++)
            g[r + i, c + j] = val;
    }
}