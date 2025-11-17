using System.Diagnostics;
using AmazingMazeDesktop.Components;
using AmazingMazeDesktop.ECS.Components;
using AmazingMazeDesktop.Systems;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;

namespace AmazingMazeDesktop.Factory;

public class EntityFactory(GameContext context, CollisionSystem collisionSystem)
{
    private World _world;

    public void SetWorld(World world)
    {
        _world = world;
    }

    public int BuildProjectile(ProjectileBuilderArgs args)
    {
        var entity = GetNewEntity();
        entity.Attach(Assets.OrangePlaceholderTexture);
        entity.Attach(new TagsComponent() { TagsList = { Tags.Projectile } });
        entity.Attach(new Transform2(args.Position, scale: args.Scale));
        entity.Attach(new MovementComponent()
        {
            Mode = MovementComponent.MovementMode.ToDirection,
            Direction = args.Direction,
            Speed = args.Velocity,
        });
        entity.Attach(new ColliderComponent(new RectangleF(args.Position,
            args.Scale * Assets.OrangePlaceholderTexture.Bounds.Size.ToVector2() * 20), entity.Id));
        AddToCollisionWorld(entity.Id);
        return entity.Id;
    }

    public int BuildEnemy(EnemyBuilderArgs args)
    {
        var entity = GetNewEntity();
        entity.Attach(new TagsComponent() { TagsList = { Tags.Enemy } });
        entity.Attach(new AnimatorComponent());
        entity.Attach(new StateComponent());
        entity.Attach(new Transform2(args.Position));
        entity.Attach(new MovementComponent() { Speed = args.Speed });
        entity.Attach(new ColliderComponent(new RectangleF(args.Position,
            new Vector2(32, 48)), entity.Id));
        AddToCollisionWorld(entity.Id);
        entity.Attach(new PathComponent());
        return entity.Id;
    }

    public int BuildSpawner(SpawnerBuilderArgs args)
    {
        var entity = GetNewEntity();
        entity.Attach(new TagsComponent() { TagsList = { Tags.Spawner } });
        entity.Attach(new Transform2(args.Position));
        entity.Attach((new SpawnerComponent(args.TimeToSpawn)));
        return entity.Id;
    }

    public int BuildTrigger(TriggerBuilderArgs args)
    {
        var entity = GetNewEntity();
        entity.Attach(new TagsComponent() { TagsList = { Tags.Trigger } });
        entity.Attach(new Transform2(args.Position));
        entity.Attach((new TriggerComponent()
        {
            Action = args.Action,
            Type = args.Type,
        }));
        entity.Attach(new ColliderComponent(new RectangleF(args.Position,
            new Vector2(EngineSettings.CellSize, EngineSettings.CellSize)), entity.Id));
        AddToCollisionWorld(entity.Id);
        return entity.Id;
    }

    public int BuildPlayer(PlayerBuilderArgs args)
    {
        var entity = GetNewEntity();
        entity.Attach(new TagsComponent() { TagsList = { Tags.Player } });
        entity.Attach(new PlayerControlComponent());
        entity.Attach(new Transform2(args.Position));
        entity.Attach(new ShooterComponent(1));
        entity.Attach(new AnimatorComponent());
        entity.Attach(new StateComponent());
        entity.Attach(new ColliderComponent(new RectangleF(args.Position,
            new Vector2(32, 48)), entity.Id));
        AddToCollisionWorld(entity.Id);
        context.PlayerEntityId = entity.Id;
        return entity.Id;
    }

    public int BuildWall(WallBuilderArgs args)
    {
        var entity = GetNewEntity();
        entity.Attach(new TagsComponent() { TagsList = { Tags.Wall } });
        entity.Attach(new Transform2(args.Position));
        entity.Attach(new ColliderComponent(new RectangleF(args.Position,
            new Vector2(64, 64)), entity.Id, "Walls"));
        AddToCollisionWorld(entity.Id);
        return entity.Id;
    }

    private Entity GetNewEntity()
    {
        return _world.CreateEntity();
    }

    private void AddToCollisionWorld(int entityId)
    {
        collisionSystem.AddEntity(entityId);
    }
}