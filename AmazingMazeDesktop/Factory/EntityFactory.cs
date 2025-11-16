using System.Diagnostics;
using AmazingMazeDesktop.Components;
using AmazingMazeDesktop.ECS.Components;
using AmazingMazeDesktop.Systems;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;

namespace AmazingMazeDesktop.Factory;

public class EntityFactory(CollisionSystem collisionSystem)
{
    private World _world;

    public void SetWorld(World world)
    {
        _world = world;
    }

    public int BuildEntity(IBuilderArgs args)
    {
        var entity = _world.CreateEntity();
        switch (args)
        {
            case ProjectileBuilderArgs builderArgs:
                BuildProjectile(entity, builderArgs);
                break;
            case EnemyBuilderArgs builderArgs:
                BuildEnemy(entity, builderArgs);
                break;
            case SpawnerBuilderArgs builderArgs:
                BuildSpawner(entity, builderArgs);
                break;
            case PlayerBuilderArgs builderArgs:
                BuildPlayer(entity, builderArgs);
                break;
            case WallBuilderArgs builderArgs:
                BuildWall(entity, builderArgs);
                break;
            case TriggerBuilderArgs builderArgs:
                BuildTrigger(entity, builderArgs);
                break;
        }

        if (entity.Has<ColliderComponent>())
            collisionSystem.AddEntity(entity.Id);

        return entity.Id;
    }

    private void BuildProjectile(Entity entity, ProjectileBuilderArgs args)
    {
        entity.Attach(Assets.OrangePlaceholderTexture);
        entity.Attach(new TagsComponent() { TagsList = { Tags.Projectile } });
        //entity.Attach(new TagsComponent() { TagsList = { Tags.Projectile } });
        entity.Attach(new Transform2(args.Position, scale: args.Scale));
        entity.Attach(new MovementComponent()
        {
            Mode = MovementComponent.MovementMode.ToDirection,
            Direction = args.Direction,
            Speed = args.Velocity,
        });
        entity.Attach(new ColliderComponent(new RectangleF(args.Position,
            args.Scale * Assets.OrangePlaceholderTexture.Bounds.Size.ToVector2() * 20), entity.Id));
    }

    private void BuildEnemy(Entity entity, EnemyBuilderArgs args)
    {
        entity.Attach(new TagsComponent() { TagsList = { Tags.Enemy } });
        entity.Attach(new AnimatorComponent());
        entity.Attach(new StateComponent());
        entity.Attach(new Transform2(args.Position));
        entity.Attach(new MovementComponent() { Speed = args.Speed });
        entity.Attach(new ColliderComponent(new RectangleF(args.Position,
            new Vector2(32, 48)), entity.Id));
        entity.Attach(new PathComponent());
    }

    private void BuildSpawner(Entity entity, SpawnerBuilderArgs args)
    {
        entity.Attach(new TagsComponent() { TagsList = { Tags.Spawner } });
        //entity.Attach(Assets.YellowPlaceholderTexture);
        entity.Attach(new Transform2(args.Position));
        //entity.Attach(new ColliderComponent(new RectangleF(args.Position,
        //    Assets.YellowPlaceholderTexture.Bounds.Size.ToVector2() * args.Scale), entity.Id));
        entity.Attach((new SpawnerComponent(args.TimeToSpawn)));
    }
    
    private void BuildTrigger(Entity entity, TriggerBuilderArgs args)
    {
        entity.Attach(new TagsComponent() { TagsList = { Tags.Trigger } });
        entity.Attach(new Transform2(args.Position));
        entity.Attach((new TriggerComponent()
        {
            Action = args.Action,
            Type = args.Type,
        }));
        entity.Attach(new ColliderComponent(new RectangleF(args.Position,
            new Vector2(EngineSettings.CellSize, EngineSettings.CellSize)), entity.Id));
    }

    private void BuildPlayer(Entity entity, PlayerBuilderArgs args)
    {
        entity.Attach(new TagsComponent() { TagsList = { Tags.Player } });
        entity.Attach(new PlayerControlComponent());
        entity.Attach(new Transform2(args.Position));
        entity.Attach(new ShooterComponent(1));
        entity.Attach(new AnimatorComponent());
        entity.Attach(new StateComponent());
        entity.Attach(new ColliderComponent(new RectangleF(args.Position,
            new Vector2(32, 48)), entity.Id));
    }

    private void BuildWall(Entity entity, WallBuilderArgs args)
    {
        entity.Attach(new TagsComponent() { TagsList = { Tags.Wall } });
        entity.Attach(new Transform2(args.Position));
        entity.Attach(new ColliderComponent(new RectangleF(args.Position,
            new Vector2(64, 64)), entity.Id, "Walls"));
    }
}