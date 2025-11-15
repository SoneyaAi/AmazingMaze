using System.Linq;
using AmazingMazeDesktop.Factory;
using AmazingMazeDesktop.Systems;
using AmazingMazeDesktop.WorldModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

    public void Initialize(GameContext context, Camera2D camera, GraphicsDevice graphicsDevice)
    {
        _collisionSystem = new CollisionSystem();
        _entityFactory = new EntityFactory(_collisionSystem);
        _spawnSystem = new SpawnSystem(_entityFactory);
        _movementSystem = new MovementSystem();
        _movementSystem.MazeStructure = context.Dungeon.Levels.First().MazeStructure;

        World = new WorldBuilder()
            .AddSystem(new PlayerControlSystem())
            .AddSystem(_spawnSystem)
            .AddSystem(new TriggersSystem(context))
            .AddSystem(new PathfindingSystem(context.Dungeon.Levels.First().MazeStructure))
            .AddSystem(_movementSystem)
            .AddSystem(new ShootingSystem(_entityFactory))
            .AddSystem(new EnemyAiSystem())
            .AddSystem(_collisionSystem)
            .AddSystem(new StateSystem())
            .AddSystem(new CameraSystem(camera))
            .AddSystem(new RenderSystem(graphicsDevice, camera))
            .Build();
        _entityFactory.SetWorld(World);
        
        // Spawns
        foreach (var spawner in context.Dungeon.Levels.First().SpawnPoints)
        {
            if (spawner.IsPlayerSpawn)
            {
                _entityFactory.BuildEntity(new PlayerBuilderArgs()
                {
                    Position = Conversions.CellToWorld(spawner.TileCoordinates)
                });
                continue;
            }

            var spawnerPos = Conversions.CellToWorld(spawner.TileCoordinates);
            _entityFactory.BuildEntity(new SpawnerBuilderArgs()
            {
                Position = spawnerPos,
                TimeToSpawn = 20
            });
        }
        
        for (int y = 0; y < context.Dungeon.Levels.First().MazeStructure.Map.GetLength(0); y++)
        {
            for (int x = 0; x < context.Dungeon.Levels.First().MazeStructure.Map.GetLength(1); x++)
            {
                if (context.Dungeon.Levels.First().MazeStructure.Map[y, x] != 1)
                    continue;

                var wallEntity = _entityFactory.BuildEntity(new WallBuilderArgs()
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