using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AmazingMazeDesktop.Components;
using Labyrinthian;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace AmazingMazeDesktop.Systems;

public class MovementSystem() : EntityUpdateSystem(Aspect.All(typeof(MovementComponent)))
{
    public MazeStructure MazeStructure;
    private ComponentMapper<Transform2> _transformMapper;
    private ComponentMapper<MovementComponent> _movementMapper;
    private ComponentMapper<StateComponent> _stateMapper;
    private ComponentMapper<PathComponent> _pathMapper;
    public static float WaypointRadius = EngineSettings.CellSize / 8;

    public override void Initialize(IComponentMapperService mapperService)
    {
        _transformMapper = mapperService.GetMapper<Transform2>();
        _movementMapper = mapperService.GetMapper<MovementComponent>();
        _stateMapper = mapperService.GetMapper<StateComponent>();
        _pathMapper = mapperService.GetMapper<PathComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var entity in ActiveEntities)
        {
            var transform = _transformMapper.Get(entity);
            var movement = _movementMapper.Get(entity);
            var position = transform.Position;

            if (movement.Mode == MovementComponent.MovementMode.FollowTarget)
            {
                var pathComponent = _pathMapper.Get(entity);
                if (pathComponent.Path.Count == 0) // Empty path
                {
                    pathComponent.RecalculateRequested = true;
                    // path = Maze.GetPath(Conversions.WorldToCell(position),
                    //     Conversions.WorldToCell(movement.Target));
                    continue;
                }

                if (Conversions.WorldToCell(movement.Target) !=
                    pathComponent.Path.LastOrDefault()) // Target position change
                {
                    pathComponent.RecalculateRequested = true;
                    continue;
                    // movement.Path = Maze.GetPath(Conversions.WorldToCell(position),
                    //     Conversions.WorldToCell(movement.Target));
                }

                //if (movement.Path.Count == 0) continue;
                if (Vector2.Distance(Conversions.CellToWorld(pathComponent.Path.Peek()), position) <
                    WaypointRadius) // Waypoint reached
                {
                    pathComponent.Path.Dequeue();
                    //if (pathComponent.Path.Count == 0) continue;
                }

                if (pathComponent.Path.TryPeek(out var nextWaypoint))
                {
                    var target = Conversions.CellToWorld(nextWaypoint);
                    //Debug.WriteLine($"Move peek: {movement.Path.Peek()}");
                    movement.Direction = Vector2.Normalize(Vector2.Subtract(target, position));
                    transform.Position += movement.Direction * movement.Speed *
                                          (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                // if (Conversions.WorldToCell(movement.Target) != movement.Path.LastOrDefault()) // Target position change
                // {
                //     movement.Path = Maze.GetPath(Conversions.WorldToCell(position),
                //         Conversions.WorldToCell(movement.Target));
                // }
                // if (movement.Path.Count == 0) continue;
                // if (Vector2.Distance(Conversions.CellToWorld(movement.Path.Peek()), position) < WaypointRadius) // Waypoint reached
                // {
                //     movement.Path.Dequeue();
                //     if (movement.Path.Count == 0) continue;
                // }


                // var target = Conversions.CellToWorld(movement.Path.Peek());
                // //Debug.WriteLine($"Move peek: {movement.Path.Peek()}");
                // movement.Direction = Vector2.Normalize(Vector2.Subtract(target, position));
                // transform.Position += movement.Direction * movement.Speed *
                //                       (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (movement.Mode == MovementComponent.MovementMode.ToDirection)
            {
                transform.Position +=
                    movement.Direction * movement.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}