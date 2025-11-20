using System;
using System.Linq;
using AmazingMazeDesktop.UI;
using AmazingMazeDesktop.WorldGeneration.Configs;
using Gum.Forms;
using Gum.Forms.Controls;
using Gum.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Input;
using MonoGame.Extended.Screens;
using MonoGameGum;

namespace AmazingMazeDesktop.Screens;

public class GameplayScreen(Game game) : GameScreen(game)
{
    private SpriteBatch _spriteBatch;
    private DebugUi _debugUi = new DebugUi(game);
    private GameContext _gameContext;
    private OrthographicCamera _camera;
    private ConfigsPackage _configs;
    private World _worldLast;

    public override void Initialize()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _camera = new OrthographicCamera(GraphicsDevice);
        base.Initialize();
    }

    public override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _configs = new ConfigsPackage()
        {
            DungeonConfig = new DungeonConfig()
            {
                Seed = 1234
            },
            LevelConfig = new LevelConfig()
            {
                Seed = 1234,
                Height = 30,
                Width = 30,
            }
        };

        Assets.Load(Content);
        Assets.OrangePlaceholderTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
        Assets.OrangePlaceholderTexture.SetData([Color.MonoGameOrange]);
        Assets.WhitePlaceholderTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
        Assets.WhitePlaceholderTexture.SetData([Color.White]);
        Assets.YellowPlaceholderTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
        Assets.YellowPlaceholderTexture.SetData([Color.Yellow]);
        Assets.GreenPlaceholderTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
        Assets.GreenPlaceholderTexture.SetData([Color.Green]);

        _gameContext = new GameContext(_configs, _camera, GraphicsDevice);
        _worldLast = _gameContext.World.World;
        //game.Components.Add(_worldLast);
        _debugUi.Initialize();
        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
        if (KeyboardExtended.GetState().WasKeyReleased(Keys.F10))
        {
            _debugUi.SwitchVisibility();
        }

        _gameContext.Update(gameTime);
        if (_worldLast != _gameContext.World.World)
        {
            
            //Components.Remove(_gameContext.World.World);
            //game.Components.Remove(_worldLast);
            _worldLast = _gameContext.World.World;
            //game.Components.Add(_gameContext.World.World);
        }

        _worldLast.Update(gameTime);
        _debugUi.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.BlanchedAlmond);
        _gameContext.Draw(gameTime);
        _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());
        for (var y = 0; y < _gameContext.CurrentLevel.MazeStructure.Map.GetLength(0); y++)
        {
            for (var x = 0; x < _gameContext.CurrentLevel.MazeStructure.Map.GetLength(1); x++)
            {
                switch (_gameContext.CurrentLevel.MazeStructure.Map[y, x])
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
                        if (_gameContext.CurrentLevel.EntryRoomId ==
                            _gameContext.CurrentLevel.MazeStructure.Map[y, x] ||
                            _gameContext.CurrentLevel.ExitRoomId == _gameContext.CurrentLevel.MazeStructure.Map[y, x])
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
        _worldLast.Draw(gameTime);
        _debugUi.Draw();
    }
}