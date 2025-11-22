using System;
using System.Linq;
using Gum.DataTypes;
using Gum.Forms;
using Gum.Forms.Controls;
using Gum.Managers;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using MonoGameGum;

namespace AmazingMazeDesktop.UI;

public class DebugUi(Game game)
{
    private GumService _ui => GumService.Default;
    private GraphicalUiElement _screen;

    public void Initialize()
    {
        // Load project 
        _ui.Initialize(game, "../UI/GUM/Debug/debug.gumx");

        if (ObjectFinder.Self.GumProjectSave == null)
            throw new Exception("Gum project not found");

        _screen = ObjectFinder.Self.GumProjectSave.Screens
            .FirstOrDefault()
            ?.ToGraphicalUiElement();

        if (_screen == null)
            throw new Exception("No screen found in the Gum project");

        _screen.AddToRoot();
        _screen.Visible = false;

        // Controls setup
        var control0 = _ui.Root.GetFrameworkElementByName<CheckBox>("CollidersCheckbox");
        control0.Click += (sender, args) => { DebugSettings.ShowColliders = (bool)control0.IsChecked; };
        control0.IsChecked = DebugSettings.ShowColliders;

        var control1 = _ui.Root.GetFrameworkElementByName<CheckBox>("PathsCheckbox");
        control1.Click += (sender, args) => { DebugSettings.ShowPaths = (bool)control1.IsChecked; };
        control1.IsChecked = DebugSettings.ShowPaths;

        var control2 = _ui.Root.GetFrameworkElementByName<CheckBox>("WaypointsCheckbox");
        control2.Click += (sender, args) => { DebugSettings.HideFoW = (bool)control2.IsChecked; };
        control2.IsChecked = DebugSettings.HideFoW;

        var control3 = _ui.Root.GetFrameworkElementByName<CheckBox>("EnableDebugCheckbox");
        control3.Click += (sender, args) => { DebugSettings.EnableDebug = (bool)control3.IsChecked; };
        control3.IsChecked = DebugSettings.EnableDebug;
    }

    public void Update(GameTime gameTime)
    {
        _ui.Update(gameTime);
    }

    public void Draw()
    {
        _ui.Draw();
    }

    public void SwitchVisibility()
    {
        _screen.Visible = !_screen.Visible;
    }

    public void SwitchVisibility(bool toState)
    {
        _screen.Visible = toState;
    }
}