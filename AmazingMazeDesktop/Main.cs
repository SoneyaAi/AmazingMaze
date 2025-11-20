using AmazingMazeDesktop.Screens;
using AmazingMazeDesktop.WorldGeneration.Configs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.ECS;
using MonoGame.Extended.Input;
using MonoGame.Extended.Screens;


namespace AmazingMazeDesktop;

public class Main : Game
{
    private GraphicsDeviceManager _graphics;
    private ScreenManager _screenManager;
    private AnimationController _spellcastAnimationController;


    //private Camera2D _camera;


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


        base.Initialize();
    }


    protected override void LoadContent()
    {
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        KeyboardExtended.Update();
        MouseExtended.Update();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        base.Draw(gameTime);
    }
}