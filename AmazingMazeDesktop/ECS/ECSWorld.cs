using System.Linq;
using AmazingMazeDesktop.ECS.Systems;
using AmazingMazeDesktop.Factory;
using AmazingMazeDesktop.Systems;
using AmazingMazeDesktop.WorldModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using MonoGame.Extended.ECS;

namespace AmazingMazeDesktop.ECS;

public class ECSWorld
{
    public World World { get; private set; }
    private CollisionSystem _collisionSystem;
    private MovementSystem _movementSystem;
    private SpawnSystem _spawnSystem;
    private EntityFactory _entityFactory;

    public void Initialize(GameContext context, OrthographicCamera camera, GraphicsDevice graphicsDevice)
    {
        _collisionSystem = new CollisionSystem(context);
        _entityFactory = new EntityFactory(context, _collisionSystem);
        _spawnSystem = new SpawnSystem(context, _entityFactory);
        _movementSystem = new MovementSystem(context.Services.CoordinatesTranslator);
        _movementSystem.MazeStructure = context.CurrentLevel.MazeStructure;

        World = new WorldBuilder()
            .AddSystem(new PlayerControlSystem())
            .AddSystem(_spawnSystem)
            .AddSystem(new TriggersSystem(context))
            .AddSystem(new PathfindingSystem(context.Services.PathfinderService, context.Services.CoordinatesTranslator))
            .AddSystem(_movementSystem)
            .AddSystem(new ShootingSystem(_entityFactory))
            .AddSystem(new EnemyAiSystem(context))
            .AddSystem(_collisionSystem)
            .AddSystem(new StateSystem(context.Services))
            .AddSystem(new CameraSystem(camera, context.Services.PlayerTracker))
            .AddSystem(new RenderSystem(graphicsDevice, camera))
            .AddSystem(new DebugRenderSystem(graphicsDevice, camera, context.Services.CoordinatesTranslator))
            .Build();
        _entityFactory.SetWorld(World);

        // Spawns
        foreach (var spawner in context.CurrentLevel.SpawnPoints)
        {
            if (spawner.IsPlayerSpawnFromLower)
            {
                _entityFactory.BuildPlayer(new PlayerBuilderArgs()
                {
                    Position = context.Services.CoordinatesTranslator.CellToWorld(spawner.TileCoordinates)
                });
                continue;
            }

            var spawnerPos = context.Services.CoordinatesTranslator.CellToWorld(spawner.TileCoordinates);
            _entityFactory.BuildSpawner(new SpawnerBuilderArgs()
            {
                Position = spawnerPos,
                TimeToSpawn = 10
            });
        }

        // Triggers
        foreach (var trigger in context.CurrentLevel.Triggers)
        {
            _entityFactory.BuildTrigger(new TriggerBuilderArgs()
            {
                Position = context.Services.CoordinatesTranslator.CellToWorld(trigger.TileCoordinates),
                Action = trigger.Action,
                Type = trigger.Type,
            });
        }

        // Walls
        for (int y = 0; y < context.CurrentLevel.MazeStructure.Map.GetLength(0); y++)
        {
            for (int x = 0; x < context.CurrentLevel.MazeStructure.Map.GetLength(1); x++)
            {
                if (context.CurrentLevel.MazeStructure.Map[y, x] != 1)
                    continue;

                var wallEntity = _entityFactory.BuildWall(new WallBuilderArgs()
                {
                    Position = new Vector2(x * EngineSettings.CellSize + EngineSettings.CellSize * 0.5f,
                        y * EngineSettings.CellSize + EngineSettings.CellSize * 0.5f),
                });
                _collisionSystem.AddEntity(wallEntity);
            }
        }
    }
    
    public void Update(GameTime gameTime)
    {
        World.Update(gameTime);
    }

    public void Draw(GameTime gameTime)
    {
        World.Draw(gameTime);
    }
}