using System.Collections.Generic;
using Labyrinthian;
using Microsoft.Xna.Framework;

namespace AmazingMazeDesktop.Components;

public class MovementComponent
{
    public enum MovementMode
    {
        FollowTarget,
        ToDirection
    }
    
    public Vector2 Direction { get; set; }
    public Vector2 Target { get; set; }
    public float Speed { get; set; } = 60;
    
    public MovementMode Mode { get; set; } = MovementMode.FollowTarget;
    public Queue<Point> Path { get; set; } = [];
}