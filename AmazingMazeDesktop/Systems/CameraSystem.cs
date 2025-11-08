using System.Linq;
using AmazingMazeDesktop.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace AmazingMazeDesktop.Systems;

public class CameraSystem(Camera2D camera) : EntityUpdateSystem(Aspect.All(typeof(PlayerControlComponent)))
{
    private ComponentMapper<Transform2> _transformMapper;
    
    public override void Initialize(IComponentMapperService mapperService)
    {
        _transformMapper = mapperService.GetMapper<Transform2>();
    }

    public override void Update(GameTime gameTime)
    {
        if (camera.MovementMode == CameraMovementMode.TrackPlayer)
        {
            var player = _transformMapper.Get(ActiveEntities.FirstOrDefault());
            camera.MoveToPosition(player.Position.X, player.Position.Y);
        }
        camera.Update();
    }
}