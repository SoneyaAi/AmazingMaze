using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AmazingMazeDesktop.Components;
using AmazingMazeDesktop.Interfaces;
using Labyrinthian;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace AmazingMazeDesktop.Systems;

public class MovementSystem(ICoordsTranslationService  coordsService) : EntityProcessingSystem(Aspect.All(typeof(MovementComponent)))
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

    public override void Process(GameTime gameTime, int entityId)
    {
        var transform = _transformMapper.Get(entityId);
        var movement = _movementMapper.Get(entityId);
        var position = transform.Position;
        if (movement.Mode == MovementComponent.MovementMode.FollowTarget)
        {
 
            var pathComponent = _pathMapper.Get(entityId);
            if (coordsService.WorldToCell(movement.Target) == coordsService.WorldToCell(position)) // Same cell as target
            {
                movement.Direction = Vector2.Normalize(Vector2.Subtract(movement.Target, position));
                transform.Position += movement.Direction * movement.Speed *
                                      (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (pathComponent.Path.Count == 0) // Empty path
            {
                pathComponent.RecalculateRequested = true;
                return;
            }

            if (coordsService.WorldToCell(movement.Target) !=
                pathComponent.Path.LastOrDefault()) // Target cell change
            {
                pathComponent.RecalculateRequested = true;
                return;
            }

            if (_stateMapper.TryGet(entityId, out var state))
            {
                if (state.CurrentStateId != EnemyStateId.Chase)
                    return;
            }
            
            if (Vector2.Distance(coordsService.CellToWorld(pathComponent.Path.Peek()), position) <
                WaypointRadius) // Waypoint reached
            {
                pathComponent.Path.Dequeue();
            }

            if (pathComponent.Path.TryPeek(out var nextWaypoint))
            {
                var target = coordsService.CellToWorld(nextWaypoint);
                movement.Direction = Vector2.Normalize(Vector2.Subtract(target, position));
                transform.Position += movement.Direction * movement.Speed *
                                      (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
        else if (movement.Mode == MovementComponent.MovementMode.ToDirection)
        {
            transform.Position +=
                movement.Direction * movement.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}