using System;
using System.Collections.Generic;
using System.Linq;
using AmazingMazeDesktop.Interfaces;
using AmazingMazeDesktop.WorldModel;
using AStarNavigator;
using AStarNavigator.Algorithms;
using AStarNavigator.Providers;
using Microsoft.Xna.Framework;
using Tile = AStarNavigator.Tile;

namespace AmazingMazeDesktop.Services;

public class AstarPathfinderService : IPathfinderService, IBlockedProvider, INeighborProvider
{
    private readonly TileNavigator _tileNavigator;
    private TileMap _tileMap => _gameContext.CurrentLevel.TileMap;
    private readonly GameContext _gameContext;

    private readonly double[,] _neighbors = new double[8, 2]
    {
        { 0.0, -1.0 }, // Up 0
        { 1.0, 0.0 }, // Right 1
        { 0.0, 1.0 }, // Down 2 
        { -1.0, 0.0 }, // Left 3
        { -1.0, -1.0 }, // Left Up 4
        { 1.0, -1.0 }, // Right Up 5
        { 1.0, 1.0 }, //  Right Down 6 
        { -1.0, 1.0 } // Left Down 7
    };

    public AstarPathfinderService(GameContext gameContext)
    {
        _tileNavigator = new TileNavigator(
            this,
            this,
            new PythagorasAlgorithm(),
            new ManhattanHeuristicAlgorithm()
        );
        _gameContext = gameContext;
    }

    public Queue<Point> GetPath(Point startPoint, Point endPoint)
    {
        var from = new Tile(startPoint.X, startPoint.Y);
        var to = new Tile(endPoint.X, endPoint.Y);

        var result = _tileNavigator.Navigate(from, to);

        return new Queue<Point>(result.Select(x => new Point((int)x.X, (int)x.Y)));
    }

    public bool IsBlocked(Tile coord)
    {
        int x = (int)coord.X;
        int y = (int)coord.Y;

        // Out-of-bounds
        if (x < 0 || y < 0)
            return true;

        if (x >= _tileMap.Tiles.GetLength(1) ||
            y >= _tileMap.Tiles.GetLength(0))
            return true;

        // Blocked tile?
        return !_tileMap.Tiles[y, x].IsPassable;
    }


    public IEnumerable<Tile> GetNeighbors(Tile tile)
    {
        var list = new List<Tile>();

        var isTopBlocked = IsBlocked(new Tile(tile.X, tile.Y - 1));
        var isRightBlocked = IsBlocked(new Tile(tile.X + 1, tile.Y));
        var isDownBlocked = IsBlocked(new Tile(tile.X, tile.Y + 1));
        var isLeftBlocked = IsBlocked(new Tile(tile.X - 1, tile.Y));
        for (int i = 0; i < _neighbors.GetLongLength(0); i++)
        {
            if (isTopBlocked && (i == 4 || i == 5))
                continue;
            if (isRightBlocked && (i == 5 || i == 6))
                continue;
            if (isDownBlocked && (i == 6 || i == 7))
                continue;
            if (isLeftBlocked && (i == 4 || i == 7))
                continue;

            list.Add(new Tile(tile.X + _neighbors[i, 0], tile.Y + _neighbors[i, 1]));
        }

        return list;
    }
}