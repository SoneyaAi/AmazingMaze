using AmazingMazeDesktop.Components;
using AmazingMazeDesktop.Factory;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Timers;

namespace AmazingMazeDesktop.Systems;

public class ShootingSystem(EntityFactory factory) : EntityUpdateSystem(Aspect.All(typeof(ShooterComponent)))
{
    private ComponentMapper<Transform2> _transformMapper;
    private ComponentMapper<ShooterComponent> _shooterMapper;

    public override void Initialize(IComponentMapperService mapperService)
    {
        _transformMapper = mapperService.GetMapper<Transform2>();
        _shooterMapper = mapperService.GetMapper<ShooterComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var entity in ActiveEntities)
        {
            var shooter = _shooterMapper.Get(entity);
            var transform = _transformMapper.Get(entity);

            shooter.Reloading.Update(gameTime);

            if (!shooter.ShootIntent) continue;
            if (shooter.Reloading.State != TimerState.Completed) continue;
            
            factory.BuildProjectile(new ProjectileBuilderArgs()
            {
                Position = transform.Position,
                Direction = Vector2.Normalize(Vector2.Subtract(shooter.TargetPosition, transform.Position)),
                Scale = new Vector2(0.25f, 0.25f),
                Velocity = 100
            });

            
            
            
            shooter.Reloading.Restart();
        }
    }
}