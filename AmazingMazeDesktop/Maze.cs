using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Labyrinthian;
using Labyrinthian.Svg;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using ToolsUtilities;

namespace AmazingMazeDesktop;

public class Maze
{
    public int[,] Map;
    public IReadOnlyDictionary<int, List<Point>> Rooms => _roomIdCellsDictionary;

    private Dictionary<int, List<Point>> _roomIdCellsDictionary = new();
    private readonly LabyrinthianMaze _labyrinthianMaze = new();

    public Maze(int width, int height, int roomsCount = 2)
    {
        var oddWidth = width % 2 == 0 ? width + 1 : width;
        var oddHeight = height % 2 == 0 ? height + 1 : height;
        Generate(oddWidth, oddHeight, roomsCount);
    }

    private void Generate(int width, int height, int roomsCount = 2)
    {
        // Generate Labyrinthian maze 
        var baseWidth = width / 2;
        var baseHeight = height / 2;
        _labyrinthianMaze.Generate(baseWidth, baseHeight, roomsCount);

        // Convert labyrinthian maze to global maze
        var convertedMaze = new int[height, width];
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                convertedMaze[y, x] = 1;
            }
        }

        for (var x = 0; x < _labyrinthianMaze.Map.Columns; x++)
        {
            for (var y = 0; y < _labyrinthianMaze.Map.Rows; y++)
            {
                var cell = _labyrinthianMaze.Map[y, x];

                var translatedY = y * 2 + 1;
                var translatedX = x * 2 + 1;
                if (cell is null)
                {
                    // Rooms
                    if (_labyrinthianMaze.RoomsMap[y, x] >= 2)
                    {
                        var id = _labyrinthianMaze.RoomsMap[y, x];

                        if (x + 1 == _labyrinthianMaze.Map.Columns ||
                            y + 1 == _labyrinthianMaze.Map.Rows)
                            continue;

                        convertedMaze[translatedY, translatedX] = id;

                        if (_labyrinthianMaze.RoomsMap[y, x + 1] == id)
                            convertedMaze[translatedY, translatedX + 1] = id;
                        else
                            convertedMaze[translatedY, translatedX + 1] = 1;

                        if (_labyrinthianMaze.RoomsMap[y + 1, x] == id ||
                            _labyrinthianMaze.RoomsMap[y + 1, x] == 1)
                            convertedMaze[translatedY + 1, translatedX] = id;
                        else
                            convertedMaze[translatedY + 1, translatedX] = 1;

                        if (_labyrinthianMaze.RoomsMap[y, x + 1] == id &&
                            _labyrinthianMaze.RoomsMap[y + 1, x] == id)
                            convertedMaze[translatedY + 1, translatedX + 1] = id;
                    }

                    continue;
                }

                // Add walls/corridors between labyrinthian cells
                convertedMaze[translatedY, translatedX] = 0;
                convertedMaze[translatedY, translatedX + 1] =
                    _labyrinthianMaze.Map.AreCellsConnected(cell, cell.DirectedNeighbors[0]) ? 0 : 1;
                convertedMaze[translatedY + 1, translatedX] =
                    _labyrinthianMaze.Map.AreCellsConnected(cell, cell.DirectedNeighbors[2]) ? 0 : 1;
            }
        }

        // Populate rooms dictionary
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var value = convertedMaze[y, x];
                if (value < 2)
                    continue;

                if (_roomIdCellsDictionary.TryGetValue(value, out var cellsList))
                {
                    cellsList.Add(new Point(x, y));
                }
                else
                {
                    _roomIdCellsDictionary.Add(value, new List<Point>() { new Point(x, y) });
                }
            }
        }

        // Add exits from rooms
        foreach (var roomId in _roomIdCellsDictionary.Keys)
        {
            var cells = _roomIdCellsDictionary[roomId].Shuffle(new Random());
            Debug.WriteLine("cells: " + string.Join(',', cells));
            foreach (var cell in cells)
            {
                var top = cell + new Point(0, -2);
                Debug.WriteLine("top: " + top + " = " + convertedMaze[top.Y, top.X]);
                var down = cell + new Point(0, 2);
                var left = cell + new Point(-2, 0);
                var right = cell + new Point(2, 0);


                if (cell.X < 2 || cell.Y < 2)
                    continue;

                if (convertedMaze[top.Y, top.X] == 0)
                {
                    convertedMaze[top.Y + 1, top.X] = roomId;
                    break;
                }
                
                if (convertedMaze[down.Y, down.X] == 0)
                {
                    convertedMaze[down.Y - 1, down.X] = roomId;
                    break;
                }
                if (convertedMaze[right.Y, right.X] == 0)
                {
                    convertedMaze[right.Y, right.X - 1] = roomId;
                    break;
                }
                
                if (convertedMaze[left.Y, left.X] == 0)
                {
                    convertedMaze[left.Y , left.X + 1] = roomId;
                    break;
                }
            }
        }

        Map = convertedMaze;
    }

    public Queue<Point> GetPath(Point start, Point destination)
    {
        var startLabyrinthianCell = TranslateGlobalCellToLabyrinthian(start);
        var destinationLabyrinthianCell = TranslateGlobalCellToLabyrinthian(destination);

        Queue<Point> waypoints = [];

        foreach (var waypoint in _labyrinthianMaze.GetPath(startLabyrinthianCell, destinationLabyrinthianCell)
                     .Select(gridPoint => new Point(gridPoint.Column * 2 + 1, gridPoint.Row * 2 + 1)))
        {
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

        // Remove unwanted pre-start waypoints
        while (waypoints.Contains(start))
            waypoints.Dequeue();

        // Remove unwanted after-destination waypoints
        if (!waypoints.LastOrDefault().Equals(destination))
            waypoints.Enqueue(destination);

        return waypoints;
    }

    private Point TranslateGlobalCellToLabyrinthian(Point global)
    {
        var x = global.X / 2;
        var y = global.Y / 2;
        return new Point(x, y);
    }
}