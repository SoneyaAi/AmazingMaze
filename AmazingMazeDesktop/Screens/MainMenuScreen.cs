using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Input;
using MonoGame.Extended.Screens;

namespace AmazingMazeDesktop.Screens;

public class MainMenuScreen(Game game) : GameScreen(game)
{
    private SpriteBatch _spriteBatch;

    public override void Initialize()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        base.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        if(KeyboardExtended.GetState().WasKeyReleased(Keys.Enter))
        {
            ScreenManager.ShowScreen(new GameplayScreen(game));
        }
    }

    public override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.YellowGreen);
        
        _spriteBatch.Begin();
        _spriteBatch.DrawCircle(new Vector2(100,100), 100, 20, Color.CornflowerBlue, 10f);
        _spriteBatch.End();
    }
}