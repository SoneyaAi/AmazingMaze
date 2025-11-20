using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Channels;
using AmazingMazeDesktop.Factory;
using AmazingMazeDesktop.Systems;
using AmazingMazeDesktop.Components;
using AmazingMazeDesktop.ECS;
using AmazingMazeDesktop.Screens;
using AmazingMazeDesktop.WorldGeneration;
using AmazingMazeDesktop.WorldGeneration.Configs;
using AmazingMazeDesktop.WorldModel;
using Gum.Forms;
using Gum.Forms.Controls;
using Gum.Managers;
using Gum.Wireframe;
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
using MonoGame.Extended.Screens;
using MonoGame.Extended.ViewportAdapters;
using MonoGameGum;
using MonoGameGum.Forms;


namespace AmazingMazeDesktop;

public class Main : Game
{
    private GraphicsDeviceManager _graphics;
    public SpriteBatch SpriteBatch;
    private ScreenManager _screenManager;
    private AnimationController _spellcastAnimationController;
    GumService DebugUI => GumService.Default;

    private OrthographicCamera _camera;

    //private Camera2D _camera;
    private GameContext _gameContext;
    private Panel debugSettingsPanel;
    private ConfigsPackage _configs;
    private World _worldLast;

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
        _screenManager = new ScreenManager();
        _screenManager.ShowScreen(new MainMenuScreen(this));
        
        Components.Add(_screenManager);
        
        GlobalRng.Initialize(12345);

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

        _camera = new OrthographicCamera(GraphicsDevice);

        InitializeGum();
        base.Initialize();
    }

    private void InitializeGum()
    {
        DebugUI.Initialize(
            this,
            "../UI/GUM/Debug/debug.gumx");

        // This assumes that your project has at least 1 screen
        var screen = ObjectFinder.Self.GumProjectSave.Screens
            .FirstOrDefault()
            ?.ToGraphicalUiElement();

        if (screen == null)
        {
            throw new Exception(
                "No screen found in the Gum project, " +
                "did you add a Screen in the Gum tool?");
        }

        screen.AddToRoot();
        
        var control = DebugUI.Root.GetFrameworkElementByName<CheckBox>("CollidersCheckbox");//.GetGraphicalUiElementByName("CollidersCheckbox");
        control.Click += (sender, args) => { DebugSettings.ShowColliders = (bool)control.IsChecked; };

    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);
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
        Components.Add(_worldLast);

        // Debug menu
        // debugSettingsPanel = new Panel();
        //
        // debugSettingsPanel.Dock(Dock.Fill);
        // debugSettingsPanel.AddToRoot();
        // StackPanel stackPanel = new StackPanel();
        // debugSettingsPanel.AddChild(stackPanel);
        // stackPanel.Orientation = Orientation.Vertical;
        // var button = new Button();
        // button.Anchor(Anchor.Top);
        // stackPanel.AddChild(button);
        // var button2 = new Button();
        // button2.Anchor(Anchor.Top);
        // stackPanel.AddChild(button2);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        KeyboardExtended.Update();
        MouseExtended.Update();
        _gameContext.Update(gameTime);
        if (_worldLast != _gameContext.World.World)
        {
            //Components.Remove(_gameContext.World.World);
            Components.Remove(_worldLast);
            _worldLast = _gameContext.World.World;
            Components.Add(_gameContext.World.World);
        }

        if (KeyboardExtended.GetState().WasKeyReleased(Keys.F10))
        {
            DebugUI.Root.Visible = !DebugUI.Root.Visible;
        }

        
        base.Update(gameTime);
        DebugUI.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
         GraphicsDevice.Clear(Color.CornflowerBlue);
        
        _gameContext.Draw(gameTime);
        
        SpriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());
        // Draw maze background
        // for (var y = 0; y < _maze.MazeSchema.GetLength(0); y++)
        // {
        //     for (var x = 0; x < _maze.MazeSchema.GetLength(1); x++)
        //     {
        //         Debug.Write(_maze.MazeSchema[y, x]);
        //     }
        //     Debug.Write("\n");
        // }
        for (var y = 0; y < _gameContext.CurrentLevel.MazeStructure.Map.GetLength(0); y++)
        {
            for (var x = 0; x < _gameContext.CurrentLevel.MazeStructure.Map.GetLength(1); x++)
            {
                switch (_gameContext.CurrentLevel.MazeStructure.Map[y, x])
                {
                    case 0:
                        SpriteBatch.Draw(Assets.WhitePlaceholderTexture, new Vector2(x, y) * EngineSettings.CellSize,
                            null, Color.White, 0f,
                            Vector2.Zero, EngineSettings.CellSize, SpriteEffects.None, 1f);
                        break;
                    case 1:
                        SpriteBatch.Draw(Assets.OrangePlaceholderTexture, new Vector2(x, y) * EngineSettings.CellSize,
                            null, Color.White, 0f,
                            Vector2.Zero, EngineSettings.CellSize, SpriteEffects.None, 1f);
                        break;
                    case >= 2:
                        if (_gameContext.CurrentLevel.EntryRoomId ==
                            _gameContext.CurrentLevel.MazeStructure.Map[y, x] ||
                            _gameContext.CurrentLevel.ExitRoomId == _gameContext.CurrentLevel.MazeStructure.Map[y, x])
                        {
                            SpriteBatch.Draw(Assets.GreenPlaceholderTexture,
                                new Vector2(x, y) * EngineSettings.CellSize,
                                null, Color.White, 0f,
                                Vector2.Zero, EngineSettings.CellSize, SpriteEffects.None, 1f);
                        }
                        else
                        {
                            SpriteBatch.Draw(Assets.YellowPlaceholderTexture,
                                new Vector2(x, y) * EngineSettings.CellSize,
                                null, Color.White, 0f,
                                Vector2.Zero, EngineSettings.CellSize, SpriteEffects.None, 1f);
                        }
        
                        break;
                }
            }
        }
        
        SpriteBatch.End();
        
         base.Draw(gameTime);
         DebugUI.Draw();
    }
}