using System.Linq;
using AmazingMazeDesktop.Components;
using AmazingMazeDesktop.Interfaces;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Input;

namespace AmazingMazeDesktop.Systems;

public class CameraSystem(OrthographicCamera camera, IPlayerTracker playerTracker) : EntityUpdateSystem(Aspect.All(typeof(PlayerControlComponent)))
{
    private ComponentMapper<Transform2> _transformMapper;
    
    public override void Initialize(IComponentMapperService mapperService)
    {
        _transformMapper = mapperService.GetMapper<Transform2>();
    }

    public override void Update(GameTime gameTime)
    {
        int scrollDelta = MouseExtended.GetState().DeltaScrollWheelValue;
         if (scrollDelta > 0)
             camera.ZoomIn(0.04f); 
         else if (scrollDelta < 0)
             camera.ZoomOut(0.04f);
        camera.LookAt(playerTracker.GetPlayerPositionWorld());

    }
}