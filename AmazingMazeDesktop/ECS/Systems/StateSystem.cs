using System.Diagnostics;
using AmazingMazeDesktop.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace AmazingMazeDesktop.Systems;

public class StateSystem() : EntityUpdateSystem(Aspect.All(typeof(StateComponent)))
{
    private ComponentMapper<StateComponent> _stateComponentMapper;
    private ComponentMapper<MovementComponent> _movementComponentMapper;
    private ComponentMapper<PlayerControlComponent> _playerControlComponentMapper;

    public override void Initialize(IComponentMapperService mapperService)
    {
        _stateComponentMapper = mapperService.GetMapper<StateComponent>();
        _movementComponentMapper = mapperService.GetMapper<MovementComponent>();
        _playerControlComponentMapper = mapperService.GetMapper<PlayerControlComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var entity in ActiveEntities)
        {
            var stateComponent = _stateComponentMapper.Get(entity);

            if (GetEntity(entity).Has<MovementComponent>())
            {
                var movementComponent = _movementComponentMapper.Get(entity);
                var direction = movementComponent.Direction;

                if (direction.X > 0)
                {
                    stateComponent.Current = MoveState.Walk;
                    stateComponent.Facing = Facing.East;
                }
                else if (direction.X < 0)
                {
                    stateComponent.Current = MoveState.Walk;
                    stateComponent.Facing = Facing.West;
                }
                else
                {
                    stateComponent.Current = MoveState.Idle;
                    stateComponent.Facing = stateComponent.Facing;
                }
            }

             if (GetEntity(entity).Has<PlayerControlComponent>())
             {
                 var control = _playerControlComponentMapper.Get(entity);
                 if (control.WalkLeftPressed)
                 {
                     stateComponent.Current = MoveState.Walk;
                     stateComponent.Facing = Facing.West;
                 }
                 if (control.WalkRightPressed)
                 {
                     stateComponent.Current = MoveState.Walk;
                     stateComponent.Facing = Facing.East;
                 }
                 if (control.WalkUpPressed)
                 {
                     stateComponent.Current = MoveState.Walk;
                     stateComponent.Facing = Facing.North;
                 }
                 if (control.WalkDownPressed)
                 {
                     stateComponent.Current = MoveState.Walk;
                     stateComponent.Facing = Facing.South;
                 }

            }
        }
    }
}