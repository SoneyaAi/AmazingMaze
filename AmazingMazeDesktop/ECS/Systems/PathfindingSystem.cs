using System.Collections.Generic;
using AmazingMazeDesktop.Components;
using AmazingMazeDesktop.Interfaces;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace AmazingMazeDesktop.Systems;

public class PathfindingSystem(IPathfinderService pathingService, MazeStructure mazeStructure)
    : EntityProcessingSystem(Aspect.All(typeof(PathComponent)))
{
    private ComponentMapper<PathComponent> _pathMapper;
    private ComponentMapper<Transform2> _transformMapper;
    private ComponentMapper<MovementComponent> _movementMapper;

    public override void Initialize(IComponentMapperService mapperService)
    {
        _pathMapper = mapperService.GetMapper<PathComponent>();
        _transformMapper = mapperService.GetMapper<Transform2>();
        _movementMapper = mapperService.GetMapper<MovementComponent>();
    }

    public override void Process(GameTime gameTime, int entityId)
    {
        var pathComponent = _pathMapper.Get(entityId);
        if (!pathComponent.RecalculateRequested)
            return;
        
        var transform = _transformMapper.Get(entityId);
        var movement = _movementMapper.Get(entityId);
        var targetCell = Conversions.WorldToCell(movement.Target);
        var startCell = Conversions.WorldToCell(transform.Position);
        var path = new Queue<Point>();

        var corridorsPath = pathingService.GetPath(startCell, targetCell);
        while (corridorsPath.Count > 0)
        {
            path.Enqueue(corridorsPath.Dequeue());
        }
        pathComponent.Path = path;
        pathComponent.RecalculateRequested = false;
    }
}