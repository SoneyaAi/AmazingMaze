using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;

namespace AmazingMazeDesktop.Components;

public class ShooterComponent
{
    public bool ShootIntent { get; set; }
    public Vector2 TargetPosition { get; set; }
    
    public CountdownTimer Reloading { get; set; }

    public ShooterComponent(float reloadTime)
    {
        Reloading = new CountdownTimer(reloadTime);
        Reloading.Start();
    }
}