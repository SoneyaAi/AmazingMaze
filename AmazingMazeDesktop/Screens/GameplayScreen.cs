using System;
using System.Linq;
using AmazingMazeDesktop.UI;
using Gum.Forms;
using Gum.Forms.Controls;
using Gum.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.Screens;
using MonoGameGum;

namespace AmazingMazeDesktop.Screens;

public class GameplayScreen(Game game) : GameScreen(game)
{
    private SpriteBatch _spriteBatch;
    private DebugUi _debugUi = new DebugUi(game);

    public override void Initialize()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _debugUi.Initialize();
        
        base.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
        if (KeyboardExtended.GetState().WasKeyReleased(Keys.F10))
        {
            _debugUi.SwitchVisibility();
        }

        _debugUi.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Green);
        _debugUi.Draw();
    }
}