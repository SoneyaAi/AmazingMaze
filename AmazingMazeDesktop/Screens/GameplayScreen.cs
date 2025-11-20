using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;

namespace AmazingMazeDesktop.Screens;

public class GameplayScreen(Game game) : GameScreen(game)
{
    private SpriteBatch _spriteBatch;
    public override void Initialize()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        base.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        
    }

    public override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Green);
    }
}