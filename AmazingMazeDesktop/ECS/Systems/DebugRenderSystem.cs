using System.Collections.Generic;
using System.Diagnostics;
using AmazingMazeDesktop.Components;
using AmazingMazeDesktop.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace AmazingMazeDesktop.ECS.Systems;

public class DebugRenderSystem(GraphicsDevice graphicsDevice, OrthographicCamera camera)
    : EntityDrawSystem(Aspect.One(typeof(Texture2D), typeof(AnimatorComponent), typeof(ColliderComponent),
        typeof(MovementComponent)))
{
    private readonly SpriteBatch _spriteBatch = new(graphicsDevice);
    private ComponentMapper<ColliderComponent> _colliderMapper;
    private ComponentMapper<PathComponent> _pathMapper;

    public override void Initialize(IComponentMapperService mapperService)
    {
        _colliderMapper = mapperService.GetMapper<ColliderComponent>();

        _pathMapper = mapperService.GetMapper<PathComponent>();
    }

    public override void Draw(GameTime gameTime)
    {
        if (!DebugSettings.EnableDebug)
            return;

        _spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());
        // Colliders
        foreach (var entity in ActiveEntities)
        {
            if (DebugSettings.ShowColliders &&
                _colliderMapper.TryGet(entity, out var colliderComponent))
            {
                _spriteBatch.DrawRectangle((RectangleF)colliderComponent.Bounds,
                    Color.Blue);
            }

            // Paths
            if (DebugSettings.ShowPaths &&
                _pathMapper.TryGet(entity, out var pathComponent) &&
                pathComponent.Path.Count > 0)
            {
                var path = pathComponent.Path.ToArray();
                for (var i = 0; i < path.Length - 1; i++)
                {
                    var start = path[i];
                    var end = path[i + 1];

                    // Waypoint
                    _spriteBatch.DrawCircle(Conversions.CellToWorld(start), MovementSystem.WaypointRadius,
                        20, Color.Blue, 5f);

                    // Path
                    _spriteBatch.DrawLine(Conversions.CellToWorld(start),
                        end.ToVector2() * EngineSettings.CellSize +
                        new Vector2(EngineSettings.CellSize / 2, EngineSettings.CellSize / 2),
                        Color.Black, thickness: 5);
                }
            }
        }

        _spriteBatch.End();
    }
}