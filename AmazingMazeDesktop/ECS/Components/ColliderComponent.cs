using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

namespace AmazingMazeDesktop.Components;

public class ColliderComponent : ICollisionActor
{
    private RectangleF _rectangle;
    public IShapeF Bounds => _rectangle;
    public CollisionEventArgs ColliisionInfo;
    public string LayerName { get; set; }

    public ColliderComponent(RectangleF rectangle, int owner, string layer = "")
    {
        _rectangle = rectangle;
        OwnerEntity = owner;
        if(!string.IsNullOrEmpty(layer))
            LayerName = layer;
    }

    public int OwnerEntity { get; set; }
    public void OnCollision(CollisionEventArgs collisionInfo)
    {
        ColliisionInfo = collisionInfo;
    }

    public void SetPosition(Vector2 position)
    {
        _rectangle.Position = position - _rectangle.Size / 2;
    }

    public void ClearCollisionInfo()
    {
        ColliisionInfo = null;
    }
}