using AmazingMazeDesktop.Components;
using AmazingMazeDesktop.Factory;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Timers;

namespace AmazingMazeDesktop.Systems;

public class SpawnSystem(GameContext context, EntityFactory factory) : EntityUpdateSystem(Aspect.All(typeof(SpawnerComponent)))
{
    private ComponentMapper<SpawnerComponent> _spawnerMapper;
    private ComponentMapper<Transform2> _transformMapper;

    public override void Initialize(IComponentMapperService mapperService)
    {
        _spawnerMapper = mapperService.GetMapper<SpawnerComponent>();
        _transformMapper = mapperService.GetMapper<Transform2>();
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var entity in ActiveEntities)
        {
            var transform = _transformMapper.Get(entity);
            var spawner = _spawnerMapper.Get(entity);
            var timer = spawner.CountdownTimer;
            if(spawner.Locked)
                continue;
            
            timer.Update(gameTime);
            if (timer.State != TimerState.Completed) 
                continue;
            
            factory.BuildEnemy(new EnemyBuilderArgs()
            {
                Position = transform.Position,
                Speed = 30
            });

            spawner.Locked = true;
            
            timer.Restart();
        }
    }
}