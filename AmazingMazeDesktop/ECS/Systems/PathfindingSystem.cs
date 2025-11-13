using System.Collections.Generic;
using AmazingMazeDesktop.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace AmazingMazeDesktop.Systems;

public class PathfindingSystem(MazeStructure mazeStructure)
    : EntityUpdateSystem(Aspect.All(typeof(PathfindingComponent), typeof(PathComponent)))
{
    private ComponentMapper<PathfindingComponent> _pathfindingMapper;
    private ComponentMapper<PathComponent> _pathMapper;
    private ComponentMapper<Transform2> _transformMapper;
    private ComponentMapper<MovementComponent> _movementMapper;

    public override void Initialize(IComponentMapperService mapperService)
    {
        _pathfindingMapper = mapperService.GetMapper<PathfindingComponent>();
        _pathMapper = mapperService.GetMapper<PathComponent>();
        _transformMapper = mapperService.GetMapper<Transform2>();
        _movementMapper = mapperService.GetMapper<MovementComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var entity in ActiveEntities)
        {
            var pathComponent = _pathMapper.Get(entity);
            var transform = _transformMapper.Get(entity);
            var movement = _movementMapper.Get(entity);
            var targetCell = Conversions.WorldToCell(movement.Target);
            var startCell = Conversions.WorldToCell(transform.Position);
            var path = new Queue<Point>();
            if (!pathComponent.RecalculateRequested)
                continue;

            var isStartInRoom = mazeStructure.Map[startCell.Y, startCell.X] > 1;
            var isTargetInRoom = mazeStructure.Map[targetCell.Y, targetCell.X] > 1;
            var targetRoom = mazeStructure.Map[targetCell.Y, targetCell.X];
            Point targetRoomEntrance  = new Point();// = maze.Rooms[maze.Map[targetCell.Y, targetCell.X]].EntryPath[0];
            var isSameRoom = mazeStructure.Map[startCell.Y, startCell.X] == mazeStructure.Map[targetCell.Y, targetCell.X];
            if (isStartInRoom) // start is in room
            {
                if (isSameRoom)
                {
                    path.Enqueue(targetCell);
                    continue;
                }
                
                var room = mazeStructure.Rooms[mazeStructure.Map[startCell.Y, startCell.X]];
                path.Enqueue(room.EntryPath[1]);
                path.Enqueue(room.EntryPath[0]);
                startCell = room.EntryPath[0];
                //pathComponent.Path = maze.GetPath(startCell, targetCell);

            }

            if (isTargetInRoom)
                targetRoomEntrance = mazeStructure.Rooms[mazeStructure.Map[targetCell.Y, targetCell.X]].EntryPath[0];
                //targetCell = maze.Rooms[maze.Map[targetCell.Y, targetCell.X]].EntryPath[0];
            
            var corridorsPath = mazeStructure.GetPath(startCell,   isTargetInRoom ? targetRoomEntrance : targetCell);
            while (corridorsPath.Count > 0)
            {
                path.Enqueue(corridorsPath.Dequeue());
            }
            
            if (isTargetInRoom) 
            {
                var room = mazeStructure.Rooms[targetRoom];
                path.Enqueue(room.EntryPath[1]);
                path.Enqueue(targetCell);
                
            }

            pathComponent.Path = path;

            pathComponent.RecalculateRequested = false;
        }
    }
}