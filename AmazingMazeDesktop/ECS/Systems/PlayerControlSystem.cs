using System.Diagnostics;
using System.Linq;
using AmazingMazeDesktop.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Input;

namespace AmazingMazeDesktop.Systems;

public class PlayerControlSystem() : EntityUpdateSystem(Aspect.All(typeof(PlayerControlComponent)))
{
    private Vector2 _offset = Vector2.Zero;
    private ComponentMapper<Transform2> _transformMapper;
    private ComponentMapper<ShooterComponent> _shooterMapper;
    private ComponentMapper<PlayerControlComponent> _playerControlMapper;
    private ComponentMapper<StateComponent> _stateComponentMapper;

    public override void Initialize(IComponentMapperService mapperService)
    {
        _transformMapper = mapperService.GetMapper<Transform2>();
        _shooterMapper = mapperService.GetMapper<ShooterComponent>();
        _playerControlMapper = mapperService.GetMapper<PlayerControlComponent>();
        _stateComponentMapper = mapperService.GetMapper<StateComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        var keyboardState = KeyboardExtended.GetState();
        var mouseState = MouseExtended.GetState();

        foreach (var entity in ActiveEntities)
        {
            var controlComponent = _playerControlMapper.Get(entity);
            var state = _stateComponentMapper.Get(entity);
            var transform = _transformMapper.Get(entity);

            _offset = Vector2.Zero;
            if (keyboardState.IsKeyDown(Keys.W))
            {
                _offset.Y -= 1;
            }

            controlComponent.WalkUpPressed = keyboardState.IsKeyDown(Keys.W);

            if (keyboardState.IsKeyDown(Keys.S))
            {
                _offset.Y += 1;
            }

            controlComponent.WalkDownPressed = keyboardState.IsKeyDown(Keys.S);

            if (keyboardState.IsKeyDown(Keys.A))
            {
                _offset.X -= 1;
            }

            controlComponent.WalkLeftPressed = keyboardState.IsKeyDown(Keys.A);

            if (keyboardState.IsKeyDown(Keys.D))
            {
                _offset.X += 1;
            }

            controlComponent.WalkRightPressed = keyboardState.IsKeyDown(Keys.D);

            transform.Position += _offset;

            var shooter = _shooterMapper.Get(entity);
            shooter.ShootIntent = mouseState.LeftButton == ButtonState.Pressed;
            shooter.TargetPosition = mouseState.Position.ToVector2();
        }
    }
}