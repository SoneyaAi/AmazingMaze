using System;
using System.Collections.Generic;
using Labyrinthian;

namespace Labyrinthian;

public class MazePath2
{
    // Decompiled with JetBrains decompiler
// Type: Labyrinthian.MazePath
// Assembly: Labyrinthian, Version=1.4.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC0B44A7-7080-4624-9C46-843AC5B5B35D
// Assembly location: /home/adam/.nuget/packages/labyrinthian/1.4.0/lib/netstandard2.1/Labyrinthian.dll
// XML documentation location: /home/adam/.nuget/packages/labyrinthian/1.4.0/lib/netstandard2.1/Labyrinthian.xml



// #nullable enable
// namespace Labyrinthian;

/// <summary>
/// Path that represents a path in the maze from entry to exit(edges with outer cell).
/// </summary>

  private MazeCell[]? _path;
  private MazeEdge m_entry;
  private MazeEdge m_exit;
  public readonly Maze Maze;

  public MazeEdge Entry
  {
    get => this.m_entry;
    set
    {
      this.m_entry = value;
      this._path = (MazeCell[]) null;
    }
  }

  public MazeEdge Exit
  {
    get => this.m_exit;
    set
    {
      this.m_exit = value;
      this._path = (MazeCell[]) null;
    }
  }

  /// <summary>
  /// Path from <see cref="P:Labyrinthian.MazePath.Entry" /> to <see cref="P:Labyrinthian.MazePath.Exit" /> found using BFS.
  /// </summary>
  public MazeCell[] Path => this._path ?? this.FindPath();

  /// <summary>Create a maze path.</summary>
  /// <param name="maze">Maze where path will be placed.</param>
  /// <param name="entry">Edge that represents an entry(second node should be outer cell).</param>
  /// <param name="exit">Edge that represents an exit(second node should be outer cell).</param>
  /// <exception cref="T:System.ArgumentException" />
  public MazePath2(Maze maze, MazeEdge entry, MazeEdge exit)
  {
    // if (entry.Cell2.IsMazePart)
    //   throw new ArgumentException("Cell2 should be outer cell.", nameof (entry));
    // if (exit.Cell2.IsMazePart)
    //   throw new ArgumentException("Cell2 should be outer cell.", nameof (exit));
    this.Maze = maze;
    this.Entry = entry;
    this.Exit = exit;
  }

  private MazeCell[] FindPath()
  {
    Queue<List<MazeCell>> mazeCellListQueue = new Queue<List<MazeCell>>(1);
    MarkedCells markedCells = new MarkedCells(this.Maze);
    mazeCellListQueue.Enqueue(new List<MazeCell>()
    {
      this.Entry.Cell1
    });
    while (mazeCellListQueue.Count > 0)
    {
      List<MazeCell> collection = mazeCellListQueue.Dequeue();
      List<MazeCell> mazeCellList1 = collection;
      MazeCell mazeCell = mazeCellList1[mazeCellList1.Count - 1];
      if (mazeCell == this.Exit.Cell1)
        return this._path = collection.ToArray();
      if (!markedCells[mazeCell])
      {
        markedCells[mazeCell] = true;
        foreach (MazeCell neighbor in mazeCell.Neighbors)
        {
          if (this.Maze.AreCellsConnected(neighbor, mazeCell))
          {
            List<MazeCell> mazeCellList2 = new List<MazeCell>(collection.Count + 1);
            mazeCellList2.AddRange((IEnumerable<MazeCell>) collection);
            mazeCellList2.Add(neighbor);
            mazeCellListQueue.Enqueue(mazeCellList2);
          }
        }
      }
    }
    throw new PathNotFoundException();
  }

  /// <summary>Recalculate the path.</summary>
  public void Recalculate() => this._path = this.FindPath();

  /// <summary>
  /// Get all vector path segments that represent this path.
  /// </summary>
  /// <param name="includeOuterCells">
  /// If <see langword="true" /> then edges with outer cells will be returned too.
  /// </param>
  public IEnumerable<PathSegment> GetSegments(bool includeOuterCells = true)
  {
    MazeCell previousCell = this.Entry.Cell1;
    if (!this.Entry.Cell2.IsMazePart)
    {
      if (includeOuterCells)
        yield return this.Maze.GetPathBetweenCells(this.Entry.Cell2, this.Entry.Cell1);
    }
    else
      yield return this.Maze.GetPathBetweenCells(this.Entry.Cell1, this.Entry.Cell2);
    MazeCell[] path = this.Path;
    for (int i = 1; i < path.Length; ++i)
    {
      yield return this.Maze.GetPathBetweenCells(previousCell, path[i]);
      previousCell = path[i];
    }
    if (this.Exit.Cell2.IsMazePart | includeOuterCells)
      yield return this.Maze.GetPathBetweenCells(this.Exit.Cell1, this.Exit.Cell2);
  }
}

