using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using AmazingMazeDesktop.Maze.Underlay;
using Labyrinthian;
using Labyrinthian.Svg;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using ToolsUtilities;

namespace AmazingMazeDesktop;

public class MazeStructure
{
    public int[,] Map;
    public IReadOnlyDictionary<int, MazeStructureRoom> Rooms => _roomsDictionary;

    private Dictionary<int, MazeStructureRoom> _roomsDictionary = new();
    private readonly UnderlayMaze _underlayMaze = new();

    public MazeStructure(int width, int height, int roomsCount = 2)
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
        _underlayMaze.Generate(baseWidth, baseHeight, roomsCount);

        // Convert labyrinthian maze to global maze
        var convertedMaze = new int[height, width];
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                convertedMaze[y, x] = 1;
            }
        }

        for (var x = 0; x < _underlayMaze.Maze.Columns; x++)
        {
            for (var y = 0; y < _underlayMaze.Maze.Rows; y++)
            {
                var cell = _underlayMaze.Maze[y, x];

                var translatedY = y * 2 + 1;
                var translatedX = x * 2 + 1;
                if (cell is null)
                {
                    // Rooms
                    if (_underlayMaze.RoomsMap[y, x] >= 2)
                    {
                        var id = _underlayMaze.RoomsMap[y, x];

                        if (x + 1 == _underlayMaze.Maze.Columns ||
                            y + 1 == _underlayMaze.Maze.Rows)
                            continue;

                        convertedMaze[translatedY, translatedX] = id;

                        if (_underlayMaze.RoomsMap[y, x + 1] == id)
                            convertedMaze[translatedY, translatedX + 1] = id;
                        else
                            convertedMaze[translatedY, translatedX + 1] = 1;

                        if (_underlayMaze.RoomsMap[y + 1, x] == id ||
                            _underlayMaze.RoomsMap[y + 1, x] == 1)
                            convertedMaze[translatedY + 1, translatedX] = id;
                        else
                            convertedMaze[translatedY + 1, translatedX] = 1;

                        if (_underlayMaze.RoomsMap[y, x + 1] == id &&
                            _underlayMaze.RoomsMap[y + 1, x] == id)
                            convertedMaze[translatedY + 1, translatedX + 1] = id;
                    }

                    continue;
                }

                // Add walls/corridors between labyrinthian cells
                convertedMaze[translatedY, translatedX] = 0;
                convertedMaze[translatedY, translatedX + 1] =
                    _underlayMaze.Maze.AreCellsConnected(cell, cell.DirectedNeighbors[0]) ? 0 : 1;
                convertedMaze[translatedY + 1, translatedX] =
                    _underlayMaze.Maze.AreCellsConnected(cell, cell.DirectedNeighbors[2]) ? 0 : 1;
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


                if (_roomsDictionary.TryGetValue(value, out var room))
                {
                    room.Cells.Add(new Point(x, y));
                }
                else
                {
                    var newRoom = new MazeStructureRoom();
                    newRoom.Id = value;
                    newRoom.Cells.Add(new Point(x, y));
                    _roomsDictionary.Add(value, newRoom);
                }
            }
        }

        // Add exits from rooms
        foreach (var roomId in _roomsDictionary.Keys)
        {
            var cells = _roomsDictionary[roomId].Cells.ToList().Shuffle(new Random());
            foreach (var cell in cells)
            {
                var top = cell + new Point(0, -2);
                var down = cell + new Point(0, 2);
                var left = cell + new Point(-2, 0);
                var right = cell + new Point(2, 0);


                if (cell.X < 2 || cell.Y < 2)
                    continue;

                if (convertedMaze[top.Y, top.X] == 0)
                {
                    convertedMaze[top.Y + 1, top.X] = roomId;
                    _roomsDictionary[roomId].EntryCell = top - cell;
                    break;
                }

                if (convertedMaze[down.Y, down.X] == 0)
                {
                    convertedMaze[down.Y - 1, down.X] = roomId;
                    _roomsDictionary[roomId].EntryCell = down - cell;
                    break;
                }

                if (convertedMaze[right.Y, right.X] == 0)
                {
                    convertedMaze[right.Y, right.X - 1] = roomId;
                    _roomsDictionary[roomId].EntryCell = right - cell;
                    break;
                }

                if (convertedMaze[left.Y, left.X] == 0)
                {
                    convertedMaze[left.Y, left.X + 1] = roomId;
                    _roomsDictionary[roomId].EntryCell = left - cell;
                    break;
                }
            }
        }

        Map = convertedMaze;
    }
}