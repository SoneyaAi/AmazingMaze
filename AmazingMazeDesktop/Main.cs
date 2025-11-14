using System.Diagnostics;
using System.Linq;
using AmazingMazeDesktop.Factory;
using AmazingMazeDesktop.Systems;
using AmazingMazeDesktop.Components;
using AmazingMazeDesktop.ECS;
using AmazingMazeDesktop.WorldGeneration;
using AmazingMazeDesktop.WorldGeneration.Configs;
using AmazingMazeDesktop.WorldModel;
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
    private AnimationController _spellcastAnimationController;

    private Camera2D _camera;

    private Dungeon _dungeon;
    private ECSWorld _ecsWorld;

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


        //int roomsCount = height * width / 200;
        //_mazeStructure = new MazeStructure(width, height, roomsCount);

        _dungeon = new DungeonGenerator().Generate(new ConfigsPackage()
        {
            DungeonConfig = new DungeonConfig()
            {
                Seed = 1234
            },
            LevelConfig = new LevelConfig()
            {
                Seed = 1234,
                Height = 30,
                Width = 20,
            }
        });


        _camera = new Camera2D(GraphicsDevice.Viewport);

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
        Assets.GreenPlaceholderTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
        Assets.GreenPlaceholderTexture.SetData([Color.Green]);

        _ecsWorld = new ECSWorld();
        _ecsWorld.Initialize(_dungeon, _camera, GraphicsDevice);
        Components.Add(_ecsWorld.World);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        KeyboardExtended.Update();
        MouseExtended.Update();
        _ecsWorld.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _ecsWorld.Draw(gameTime);

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
        for (var y = 0; y < _dungeon.Levels.First().MazeStructure.Map.GetLength(0); y++)
        {
            for (var x = 0; x < _dungeon.Levels.First().MazeStructure.Map.GetLength(1); x++)
            {
                switch (_dungeon.Levels.First().MazeStructure.Map[y, x])
                {
                    case 0:
                        _spriteBatch.Draw(Assets.WhitePlaceholderTexture, new Vector2(x, y) * EngineSettings.CellSize,
                            null, Color.White, 0f,
                            Vector2.Zero, EngineSettings.CellSize, SpriteEffects.None, 1f);
                        break;
                    case 1:
                        _spriteBatch.Draw(Assets.OrangePlaceholderTexture, new Vector2(x, y) * EngineSettings.CellSize,
                            null, Color.White, 0f,
                            Vector2.Zero, EngineSettings.CellSize, SpriteEffects.None, 1f);
                        break;
                    case >= 2:
                        if (_dungeon.Levels.First().EntryRoomId == _dungeon.Levels.First().MazeStructure.Map[y, x] ||
                            _dungeon.Levels.First().ExitRoomId == _dungeon.Levels.First().MazeStructure.Map[y, x])
                        {
                            _spriteBatch.Draw(Assets.GreenPlaceholderTexture,
                                new Vector2(x, y) * EngineSettings.CellSize,
                                null, Color.White, 0f,
                                Vector2.Zero, EngineSettings.CellSize, SpriteEffects.None, 1f);
                        }
                        else
                        {
                            _spriteBatch.Draw(Assets.YellowPlaceholderTexture,
                                new Vector2(x, y) * EngineSettings.CellSize,
                                null, Color.White, 0f,
                                Vector2.Zero, EngineSettings.CellSize, SpriteEffects.None, 1f);
                        }

                        break;
                }
            }
        }

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}