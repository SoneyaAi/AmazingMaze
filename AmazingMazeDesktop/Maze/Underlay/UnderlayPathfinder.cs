using System;
using System.Collections.Generic;
using System.Linq;
using Labyrinthian;
using Microsoft.Xna.Framework;

namespace Labyrinthian;

public class UnderlayPathfinder
{
    public List<GridPoint2D> Path
    {
        get { return path.Select(x => ((GridMaze2D)_maze).GetCellPosition(x)).ToList(); }
    }
    
    private MazeCell[]? _path;

    private MazeEdge _entry;
    private MazeEdge _exit;
    private Maze _maze;

    private MazeCell[] path => _path ?? FindPath();


    
    public UnderlayPathfinder(GridMaze2D maze, Point startCell, Point destCell)
    {
        MazeEdge entry = new(maze[startCell.Y, startCell.X],
            maze[startCell.Y, startCell.X].DirectedNeighbors[3]!);
        MazeEdge exit = new(maze[destCell.Y, destCell.X], maze[destCell.Y, destCell.X].DirectedNeighbors[2]!);


        _maze = maze;
        _entry = entry;
        _exit = exit;
    }

    private MazeCell[] FindPath()
    {
        var mazeCellListQueue = new Queue<List<MazeCell>>(1);
        var markedCells = new MarkedCells(_maze);
        mazeCellListQueue.Enqueue([_entry.Cell1]);
        while (mazeCellListQueue.Count > 0)
        {
            var collection = mazeCellListQueue.Dequeue();
            var mazeCell = collection[^1];
            if (mazeCell == _exit.Cell1)
                return _path = collection.ToArray();
            if (!markedCells[mazeCell])
            {
                markedCells[mazeCell] = true;
                foreach (var neighbor in mazeCell.Neighbors)
                {
                    if (_maze.AreCellsConnected(neighbor, mazeCell))
                    {
                        var mazeCellList2 = new List<MazeCell>(collection.Count + 1);
                        mazeCellList2.AddRange(collection);
                        mazeCellList2.Add(neighbor);
                        mazeCellListQueue.Enqueue(mazeCellList2);
                    }
                }
            }
        }

        throw new PathNotFoundException();
    }
}