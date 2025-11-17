using System.Linq;
using AmazingMazeDesktop.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace AmazingMazeDesktop.Systems;

public class EnemyAiSystem(GameContext context) : EntityUpdateSystem(Aspect.One(typeof(TagsComponent)))
{
    private ComponentMapper<Transform2> _transform2Mapper;
    private ComponentMapper<MovementComponent> _movementComponentMapper;
    private ComponentMapper<TagsComponent> _tagsMapper;
    
    public override void Initialize(IComponentMapperService mapperService)
    {
        _transform2Mapper = mapperService.GetMapper<Transform2>();
        _movementComponentMapper = mapperService.GetMapper<MovementComponent>();
        _tagsMapper = mapperService.GetMapper<TagsComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var entity in ActiveEntities.Where( e => _tagsMapper.Get(e).HasTag(Tags.Enemy) ))
        {
            var movement = _movementComponentMapper.Get(entity);
            movement.Target = context.Services.PlayerTracker.GetPlayerPositionWorld();
        }
    }
}