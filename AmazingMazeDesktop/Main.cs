using System.Diagnostics;
using System.Linq;
using AmazingMazeDesktop.Factory;
using AmazingMazeDesktop.Systems;
using AmazingMazeDesktop.Components;
using Labyrinthian;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Collections;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Input;

namespace AmazingMazeDesktop;

public class Main : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private World _world;
    private AnimationController _spellcastAnimationController;
    private EntityFactory _entityFactory;
    private MazeStructure _mazeStructure;
    private Camera2D _camera;
    private CollisionSystem _collisionSystem;
    private MovementSystem _movementSystem;
    private SpawnSystem _spawnSystem;
    
    private int scale = 64;

    public Main()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.PreferredBackBufferWidth = 1920;
        
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
    }

    protected override void Initialize()
    {
        GlobalRng.Initialize(12345);
        
        _collisionSystem = new CollisionSystem();
        _entityFactory = new EntityFactory(_collisionSystem);
        int width = 20;
        int height = 50;
        int roomsCount = height * width / 100;
        _mazeStructure = new MazeStructure(width, height, roomsCount);
 
        _movementSystem = new MovementSystem();
        _spawnSystem = new SpawnSystem(_entityFactory);
        _camera = new Camera2D(GraphicsDevice.Viewport);
        _movementSystem.MazeStructure = _mazeStructure;
        _world = new WorldBuilder()
            .AddSystem(new PlayerControlSystem())
            .AddSystem(_spawnSystem)
            .AddSystem(new PathfindingSystem(_mazeStructure))
            .AddSystem(_movementSystem)
            .AddSystem(new ShootingSystem(_entityFactory))
            .AddSystem(new EnemyAiSystem())
            .AddSystem(_collisionSystem)
            .AddSystem(new StateSystem())
            .AddSystem(new CameraSystem(_camera))
            .AddSystem(new RenderSystem(GraphicsDevice, _camera))
            .Build();

        Components.Add(_world);
        _entityFactory.SetWorld(_world);
  
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Assets.Load(Content);
        Assets.OrangePlaceholderTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
        Assets.OrangePlaceholderTexture.SetData([Color.MonoGameOrange]);
        Assets.WhitePlaceholderTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
        Assets.WhitePlaceholderTexture.SetData([Color.White]);
        Assets.YellowPlaceholderTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
        Assets.YellowPlaceholderTexture.SetData([Color.Yellow]);
        
        for (int y = 0; y < _mazeStructure.Map.GetLength(0); y++)
        {
            for (int x = 0; x < _mazeStructure.Map.GetLength(1); x++)
            {
                if (_mazeStructure.Map[y, x] != 1)
                    continue;

                var wallEntity = _entityFactory.BuildEntity(new WallBuilderArgs()
                {
                    Position = new Vector2(x * scale + 32, y * scale + 32),
                });
                _collisionSystem.AddEntity(wallEntity);
            }
        }
        _entityFactory.BuildEntity(new PlayerBuilderArgs()
        {
            Position = new Vector2(100, 100),
        });
        foreach (var room in _mazeStructure.Rooms)
        {
            var spawnerPos = Conversions.CellToWorld(room.Value.Cells.ToList().Shuffle(GlobalRng.Random).First());
            _entityFactory.BuildEntity(new SpawnerBuilderArgs()
            {
                Position = spawnerPos,
                TimeToSpawn = 10
            });
        }
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        KeyboardExtended.Update();
        MouseExtended.Update();
        _world.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _world.Draw(gameTime);

        _spriteBatch.Begin(transformMatrix: _camera.GetTransformation());
        // Draw maze background
        // for (var y = 0; y < _maze.MazeSchema.GetLength(0); y++)
        // {
        //     for (var x = 0; x < _maze.MazeSchema.GetLength(1); x++)
        //     {
        //         Debug.Write(_maze.MazeSchema[y, x]);
        //     }
        //     Debug.Write("\n");
        // }
        for (var y = 0; y < _mazeStructure.Map.GetLength(0); y++)
        {
            for (var x = 0; x < _mazeStructure.Map.GetLength(1); x++)
            {
                switch (_mazeStructure.Map[y, x])
                {
                    case 0:
                        _spriteBatch.Draw(Assets.WhitePlaceholderTexture, new Vector2(x, y) * EngineSettings.CellSize,
                            null, Color.White, 0f,
                            Vector2.Zero, scale, SpriteEffects.None, 1f);
                        break;
                    case 1:
                        _spriteBatch.Draw(Assets.OrangePlaceholderTexture, new Vector2(x, y) * EngineSettings.CellSize,
                            null, Color.White, 0f,
                            Vector2.Zero, scale, SpriteEffects.None, 1f);
                        break;
                    case >= 2:
                        _spriteBatch.Draw(Assets.YellowPlaceholderTexture, new Vector2(x, y) * EngineSettings.CellSize,
                            null, Color.White, 0f,
                            Vector2.Zero, scale, SpriteEffects.None, 1f);
                        break;
                }
            }
        }

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}