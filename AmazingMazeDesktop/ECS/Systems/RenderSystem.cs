using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AmazingMazeDesktop.Components;
using AmazingMazeDesktop.ECS.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;
using RenderingLibrary;

namespace AmazingMazeDesktop.Systems;

public class RenderSystem(GraphicsDevice graphicsDevice, Camera2D camera)
    : EntityDrawSystem(Aspect.One(typeof(Texture2D), typeof(AnimatorComponent), typeof(ColliderComponent), typeof(MovementComponent)))
{
    private SpriteBatch _spriteBatch = new(graphicsDevice);
    private ComponentMapper<Transform2> _transformMapper;
    private ComponentMapper<Texture2D> _texture2DMapper;
    private ComponentMapper<AnimatorComponent> _animatorMapper;
    private ComponentMapper<ColliderComponent> _colliderMapper;
    private ComponentMapper<StateComponent> _stateMapper;
    private ComponentMapper<MovementComponent> _movementMapper;
    private ComponentMapper<PathComponent> _pathMapper;
    private ComponentMapper<TagsComponent> _tagsMapper;
    private Vector2 _playerPosition;
    private ComponentMapper<TriggerComponent> _triggerMapper;
    
    public override void Initialize(IComponentMapperService mapperService)
    {
        _transformMapper = mapperService.GetMapper<Transform2>();
        _texture2DMapper = mapperService.GetMapper<Texture2D>();
        _colliderMapper = mapperService.GetMapper<ColliderComponent>();
        _animatorMapper = mapperService.GetMapper<AnimatorComponent>();
        _stateMapper = mapperService.GetMapper<StateComponent>();
        _movementMapper = mapperService.GetMapper<MovementComponent>();
        _pathMapper = mapperService.GetMapper<PathComponent>();
        _tagsMapper = mapperService.GetMapper<TagsComponent>();
        _triggerMapper = mapperService.GetMapper<TriggerComponent>();
    }

    public override void Draw(GameTime gameTime)
    {
        _spriteBatch.Begin(transformMatrix: camera.GetTransformation());
        foreach (var entity in ActiveEntities)
        {
            if (_tagsMapper.TryGet(entity, out var tags))
            {
                if (tags.TagsList.Contains(Tags.Player))
                {
                    _playerPosition = _transformMapper.Get(entity).Position;
                }
            }
            
            
            if (_transformMapper.TryGet(entity, out var transform))
            {
                if (_texture2DMapper.TryGet(entity, out var texture2D))
                {
                    _spriteBatch.Draw(texture2D,
                        transform.Position,
                        null,
                        Color.White,
                        transform.Rotation,
                        texture2D.Bounds.Size.ToVector2() / 2,
                        transform.Scale * 20,
                        SpriteEffects.None,
                        1
                    );
                }

                if (_animatorMapper.TryGet(entity, out var animator))
                {
                    foreach (var aspr in animator.AnimatedSprites)
                    {
                        var state = _stateMapper.Get(entity);
                        string animName = "";
                        if (state.Current == MoveState.Walk)
                            animName = "walk";
                        else
                            animName = "spellcast";
                        
                        switch (state.Facing)
                        {
                            case Facing.North:
                                animName += "_n";
                                break;
                            case Facing.South:
                                animName += "_s";
                                break;
                            case Facing.East:
                                animName += "_e";
                                break;
                            case Facing.West:
                                animName += "_w";
                                break;
                        }
                        
                        
                        
                        if (state.Current == MoveState.Walk && aspr.CurrentAnimation != animName)
                            aspr.SetAnimation(animName);

                        aspr.Update(gameTime);
                        _spriteBatch.Draw(aspr, transform);
                    }
                }
                
    
                

                // Debug
                if (_colliderMapper.TryGet(entity, out var collider))
                {
                    if (_triggerMapper.TryGet(entity, out var trigger))
                    {
                        if (trigger.Action == TriggerAction.LoadNextLevel)
                        {
                            _spriteBatch.DrawRectangle((RectangleF)collider.Bounds,
                                Color.White);
                        }
                        if (trigger.Action == TriggerAction.LoadPreviousLevel)
                        {
                            _spriteBatch.DrawRectangle((RectangleF)collider.Bounds,
                                Color.Red);
                        }
                    }
                    else
                    {
                        _spriteBatch.DrawRectangle((RectangleF)collider.Bounds,
                            Color.Blue);
                    }
                }

                if (_pathMapper.TryGet(entity, out var pathCompoennt) && pathCompoennt.Path.Count > 0)
                {
                        var path = new Queue<Point>(pathCompoennt.Path);
                        try
                        {
                            while (path.Count > 0)
                            {
                                var first = path.Dequeue();
                                var next = path.Peek().ToVector2();
                                _spriteBatch.DrawCircle(Conversions.CellToWorld(first),MovementSystem.WaypointRadius,20,Color.Blue,5f);
                                
                                _spriteBatch.DrawLine(Conversions.CellToWorld(first), next * 64 + new Vector2(32, 32),
                                    Color.Black, thickness:5);
                                
                            }
                        }
                        catch
                        {
                        }
                }
                
                
                
            }
        }

        _spriteBatch.End();
        
        // FOW
        // var vp = graphicsDevice.Viewport;
        // var screenRect = new Rectangle(0, 0, vp.Width, vp.Height);
        //
        // Vector2 playerScreenPos = camera.WorldSpaceToScreen(_playerPosition);
        //
        // Vector2 playerPosUV = new Vector2(
        //     playerScreenPos.X / screenRect.Width,
        //     playerScreenPos.Y / screenRect.Height);
        //
        //
        // Assets.FogOfWarEffect.Parameters["FogColor"].SetValue(new Vector4(0f, 0f, 0f, 1f));
        // Assets.FogOfWarEffect.Parameters["PlayerPosPixels"].SetValue(playerScreenPos);
        // Assets.FogOfWarEffect.Parameters["Radius"].SetValue(128);
        // Assets.FogOfWarEffect.Parameters["Softness"].SetValue(128);
        // Assets.FogOfWarEffect.Parameters["TextureWidth"].SetValue(vp.Width);
        // Assets.FogOfWarEffect.Parameters["TextureHeight"].SetValue(vp.Height);
        // Assets.FogOfWarEffect.Parameters["Zoom"].SetValue(camera.Zoom);
        // _spriteBatch.Begin(
        //     
        //     SpriteSortMode.Immediate,
        //     BlendState.AlphaBlend,
        //     SamplerState.PointClamp,
        //     DepthStencilState.None,
        //     RasterizerState.CullNone,
        //     effect: Assets.FogOfWarEffect
        // );

        //Debug.WriteLine($"fow: {_playerPosition}");
        // rysujemy po prostu biały prostokąt na cały ekran
        // _spriteBatch.Draw(Assets.WhitePlaceholderTexture, screenRect, Color.White);
        // _spriteBatch.End();
        
    }
    
    
}