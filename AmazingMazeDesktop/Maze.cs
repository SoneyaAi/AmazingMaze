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
    private OrthogonalMaze _maze;

    public Maze(int width, int height)
    {
        GenerateMaze(width, height);
    }

    public void GenerateMaze(int width, int height, int roomsCount = 2)
    {
        // Generate Labyrinthian maze 
        var excluded = new List<GridPoint2D>();
        var rooms = GetRoomsMap(height, width, roomsCount);
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
        _maze = new OrthogonalMaze(width, height, excludedPredicate);
        var generator = new PrimGeneration(_maze);
        generator.Generate();

        // Generate converted maze schema
        var H = height * 2 + 1;
        var W = width * 2 + 1;
        var convertedMaze = new int[H, W];
        for (var x = 0; x < W; x++)
        {
            for (var y = 0; y < H; y++)
            {
                convertedMaze[y, x] = 1;
            }
        }

        for (var x = 0; x < _maze.Columns; x++)
        {
            for (var y = 0; y < _maze.Rows; y++)
            {
                var cell = _maze[y, x];

                var translatedY = y * 2 + 1;
                var translatedX = x * 2 + 1;
                if (cell is null)
                {
                    if (rooms[y, x] >= 2)
                    {
                        var id = rooms[y, x];
                        if (x + 1 == _maze.Columns || y + 1 == _maze.Rows)
                            continue;
                        convertedMaze[translatedY, translatedX] = id;
                        if (rooms[y, x + 1] == id)
                            convertedMaze[translatedY, translatedX + 1] = id;
                        else
                            convertedMaze[translatedY, translatedX + 1] = 1;

                        if (rooms[y + 1, x] == id)
                            convertedMaze[translatedY + 1, translatedX] = id;
                        else if (rooms[y + 1, x] == 1)
                            convertedMaze[translatedY + 1, translatedX] = id;
                        else
                            convertedMaze[translatedY + 1, translatedX] = 1;

                        if (rooms[y, x + 1] == id && rooms[y + 1, x] == id)
                            convertedMaze[translatedY + 1, translatedX + 1] = id;
                    }

                    continue;
                }

                convertedMaze[translatedY, translatedX] = 0;
                convertedMaze[translatedY, translatedX + 1] =
                    _maze.AreCellsConnected(cell, cell.DirectedNeighbors[0]) ? 0 : 1;
                convertedMaze[translatedY + 1, translatedX] =
                    _maze.AreCellsConnected(cell, cell.DirectedNeighbors[2]) ? 0 : 1;
            }
        }

        MazeSchema = convertedMaze;

        MazeSvgExporterBuilder.For(_maze)
            .AddBackground(SvgColor.White)
            .AddWallsAsSinglePath()
            .Build()
            .ExportToFile("orthogonal-maze.svg");

        for (var y = 0; y < convertedMaze.GetLength(0); y++)
        {
            for (var x = 0; x < convertedMaze.GetLength(1); x++)
            {
                Debug.Write(convertedMaze[y, x]);
            }

            Debug.Write("\n");
        }

        Debug.WriteLine("==========================================================================");
    }

    public Queue<Point> GetPath(Point start, Point destination)
    {
        var startCell = TranslateGlobalToCell(start);
        var destCell = TranslateGlobalToCell(destination);

        if (startCell.X < 0)
            return [];
        MazeEdge northWall = new(_maze[startCell.Y, startCell.X],
            _maze[startCell.Y, startCell.X].DirectedNeighbors[3]!);
        MazeEdge southWall = new(_maze[destCell.Y, destCell.X], _maze[destCell.Y, destCell.X].DirectedNeighbors[2]!);
        var path = new MazePath2(_maze, northWall, southWall);
        Queue<Point> waypoints = [];

        var pathList = path.Path.ToList();
        foreach (var cell in pathList)
        {
            var gridPoint = _maze.GetCellPosition(cell);
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
            //Debug.WriteLine(_maze.GetCellPosition(cell).Column + " " + _maze.GetCellPosition(cell).Row);
        }
        if(!waypoints.Last().Equals(destination))
            waypoints.Enqueue(destination);
        while (waypoints.Contains(start))
            waypoints.Dequeue();
        
        return waypoints;
    }

    private Point TranslateGlobalToCell(Point global)
    {
        var x = global.X / 2;
        var y = global.Y / 2;
        return new Point(x, y);
    }

    // Rooms creation
    private int[,] GetRoomsMap(int width, int height, int roomsCount)
    {
        var map = new int[width, height];
        var numberOfRooms = width * height / 30;

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